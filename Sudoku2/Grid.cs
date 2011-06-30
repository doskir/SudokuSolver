using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sudoku
{
    class Grid
    {
        bool IsSolved
        {
            get
            {
                foreach (Cell cell in Cells)
                    if (cell.Value == Cell.CellValue.None)
                        return false;

                return IsValid;
            }
        }
        bool IsValid
        {
            get
            {
                foreach (Cell cell in Cells)
                {
                    foreach (Cell relatedCell in cell.RelatedCells)
                    {
                        if (cell.Value != Cell.CellValue.None && cell.Value == relatedCell.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public int SideLength;
        public Cell[,] Cells;
        public List<Cell> PresolvedCells = new List<Cell>();
        public List<Cell> SolvedCells = new List<Cell>();
        public Grid(string gridstring)
        {
            //magic numbers
            if (gridstring.Length < 16 || gridstring.Length > 625)
            {
                throw new Exception("gridstring too short, expected 16,81,256 or 625 got " + gridstring.Length);
            }
            //[ROW,COLUMN]
            SideLength = (int)Math.Sqrt(gridstring.Length);
            Cells = new Cell[SideLength, SideLength];
            string parsedString = gridstring.Replace("\n", "").Replace(" ", "0");
            int i = 0;
            for (int row = 0; row < SideLength; row++)
            {
                for (int column = 0; column < SideLength; column++)
                {
                    int number = -1;
                    if (parsedString[i] >= 65)
                    {
                        //parse letter to number
                        if (parsedString[i] <= 90)
                            number = parsedString[i] - 64;
                        else if (parsedString[i] >= 97 && parsedString[i] <= 122)
                            number = parsedString[i] - 96;
                    }
                    else
                        number = int.Parse(parsedString[i].ToString());

                    Cells[row, column] = new Cell(row, column, number);
                    if (number != 0)
                        PresolvedCells.Add(Cells[row, column]);
                    i++;
                }
            }

            foreach (Cell cell in Cells)
                cell.Initialize(Cells,SideLength);
        }
        void UpdateCells()
        {
            foreach (Cell cell in Cells)
                cell.UpdatePossibleNumbers();
        }
        public bool Solve()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int uniques = FindUniques();
            Console.WriteLine(uniques + " uniques found.");
            if (IsSolved)
            {
                sw.Stop();
                Console.WriteLine("Solving took " + sw.ElapsedMilliseconds + "ms.");
                return true;
            }
            Bruteforce();
            foreach (Cell cell in Cells)
                if (cell.Value != Cell.CellValue.None && !SolvedCells.Contains(cell) && !PresolvedCells.Contains(cell))
                    SolvedCells.Add(cell);
            int bruteforced = SolvedCells.Count - uniques;
            Console.WriteLine(bruteforced + " cells bruteforced.");
            sw.Stop();
            Console.WriteLine("Solving took " + sw.ElapsedMilliseconds + "ms.");
            return IsSolved;
        }
        int FindUniques()
        {
            int uniquesFound = 0;
            bool cellSolved;
            do
            {
                cellSolved = false;
                UpdateCells();
                foreach (Cell cell in Cells)
                {
                    if (!cell.Solved)
                    {
                        if (cell.Solve())
                        {
                            cellSolved = true;
                            SolvedCells.Add(cell);
                            uniquesFound++;
                            break;
                        }
                    }
                }
            } while (cellSolved);
            return uniquesFound;
        }
        public string GridString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for(int row = 0;row < SideLength;row++)
                    for (int column = 0; column < SideLength; column++)
                    {
                        if (SideLength > 9 && Cells[row,column].Value != Cell.CellValue.None)
                            sb.Append((char) ('@' + Cells[row, column].Value));
                        else
                            sb.Append((int) Cells[row, column].Value);
                    }
                return sb.ToString();
            }
        }
        void Bruteforce()
        {
            UpdateCells();
            bool backstep = false;
            for (int row = 0; row < SideLength; row++)
            {
                for (int column = 0; column < SideLength; )
                {
                    Cell cell = Cells[row, column];
                    if (!cell.Solved)
                    {
                        if (cell.PossibleNumbers.Count == 1)
                        {
                            cell.Value = cell.PossibleNumbers[0];
                            cell.Solved = true;
                            if (!IsValid)
                                backstep = true;
                        }
                        else
                        {
                            //indexof returns -1 when not found, we turn this into 0
                            //if its already using a valid number we want to try the next one
                            int i = cell.PossibleNumbers.IndexOf(cell.Value) + 1;

                            do
                            {
                                if (i == cell.PossibleNumbers.Count)
                                {
                                    cell.Value = Cell.CellValue.None;
                                    backstep = true;
                                    break;
                                }
                                cell.Value = cell.PossibleNumbers[i];
                                i++;
                            } while (!IsValid);
                        }
                    }
                    if (!backstep)
                        column++;
                    else
                        while (true)
                        {
                            column--;
                            if (column < 0)
                            {
                                column = SideLength - 1;
                                row--;
                            }
                            if (!Cells[row, column].Solved || (row == 0 && column == 0))
                            {
                                backstep = false;
                                break;
                            }
                        }
                }
            }
        }
    }
}
