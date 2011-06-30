using System;
using System.Collections.Generic;
using System.Linq;
namespace Sudoku
{
    class Cell
    {
        public enum CellValue
        {
            None,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Eleven,
            Twelve,
            Thirteen,
            Fourteen,
            Fifteen,
            Sixteen,
            Seventeen,
            Eighteen,
            Nineteen,
            Twenty,
            TwentyOne,
            TwentyTwo,
            TwentyThree,
            TwentyFour,
            TwentyFive
        } ;
        public bool Solved;
        public CellValue Value;
        public List<CellValue> AvailableNumbers;
        public int Row;
        public int Column;
        public List<CellValue> PossibleNumbers = new List<CellValue>();
        public List<Cell> RelatedCells;
        int _subgridWidth;
        public Cell(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = (CellValue) value;
            if (value > 0)
                Solved = true;
        }
        public void Initialize(Cell[,] parentGrid, int sidelength)
        {
            _subgridWidth = (int) Math.Sqrt(sidelength);
            AvailableNumbers = new List<CellValue>(Enum.GetValues(typeof (CellValue)).Cast<CellValue>());
            if (sidelength < 25)
                for (int i = 25; i >= sidelength + 1; i--)
                    AvailableNumbers.RemoveAt(i);
            AvailableNumbers.Remove(CellValue.None);
            RelatedCells = GetRelatedCells(sidelength,parentGrid);
        }

        List<Cell> GetRelatedCells(int sideLength,Cell[,] parentGrid)
        {
            List<Cell> cells = new List<Cell>();
            for (int column = 0; column < sideLength; column++)
            {
                if (column == Column)
                    continue;
                cells.Add(parentGrid[Row, column]);
            }
            for (int row = 0; row < sideLength; row++)
            {
                if (row == Row)
                    continue;
                cells.Add(parentGrid[row, Column]);
            }
            int subGridWidth = _subgridWidth;
            int subGridRowStart = Row / subGridWidth * subGridWidth;
            int subGridColumnStart = Column / subGridWidth * subGridWidth;
            for (int row = subGridRowStart; row < subGridRowStart + subGridWidth; row++)
            {
                for (int column = subGridColumnStart; column < subGridColumnStart + subGridWidth; column++)
                {
                    if (row == Row && column == Column)
                        continue;
                    if (!cells.Contains(parentGrid[row, column]))
                        cells.Add(parentGrid[row, column]);
                }
            }
            return cells;
        }
        public void UpdatePossibleNumbers()
        {
            ;
            if (Solved)
                PossibleNumbers = new List<CellValue>();
            else
            {
                PossibleNumbers = AvailableNumbers;
                
                foreach (Cell cell in RelatedCells.FindAll(c => c.Value != CellValue.None))
                    PossibleNumbers.Remove(cell.Value);
            }
        }
        public bool Solve()
        {
            if (Solved)
                return false;
            if (PossibleNumbers.Count == 0)
                throw new Exception("Row:" + Row + ",Column:" + Column + " not solved and no possible numbers.");
            if (PossibleNumbers.Count == 1)
            {
                Value = PossibleNumbers[0];
                Solved = true;
                return true;
            }
            return FindUniquePossibleNumberInAllRelatedCells() || FindUniqueInRow() || 
                   FindUniqueInColumn() || FindUniqueInSubgrid();
        }
        bool FindUniquePossibleNumberInAllRelatedCells()
        {
            List<CellValue> uniqueNumbers = new List<CellValue>(PossibleNumbers);
            foreach (Cell cell in RelatedCells)
                foreach (CellValue number in cell.PossibleNumbers)
                    uniqueNumbers.Remove(number);

            if (uniqueNumbers.Count == 1)
            {
                Value = uniqueNumbers[0];
                Solved = true;
                return true;
            }
            if (uniqueNumbers.Count > 1)
                throw new Exception("Row:" + Row + ",Column:" + Column + " more than 1 unique number.");
            return false;
        }
        bool FindUniqueInRow()
        {
            List<CellValue> uniqueNumbers = new List<CellValue>(PossibleNumbers);
            foreach (Cell cell in RelatedCells.FindAll(c => c.Row == Row))
                foreach (CellValue value in cell.PossibleNumbers)
                    uniqueNumbers.Remove(value);

            if (uniqueNumbers.Count == 1)
            {
                Value = uniqueNumbers[0];
                Solved = true;
                return true;
            }
            if (uniqueNumbers.Count > 1)
                throw new Exception("Row:" + Row + ",Column:" + Column + " more than 1 unique number.");
            return false;
        }
        bool FindUniqueInColumn()
        {
            List<CellValue> uniqueNumbers = new List<CellValue>(PossibleNumbers);
            foreach (Cell cell in RelatedCells.FindAll(c => c.Column == Column))
                foreach (CellValue value in cell.PossibleNumbers)
                    uniqueNumbers.Remove(value);

            if (uniqueNumbers.Count == 1)
            {
                Value = uniqueNumbers[0];
                Solved = true;
                return true;
            }
            if (uniqueNumbers.Count > 1)
                throw new Exception("Row:" + Row + ",Column:" + Column + " more than 1 unique number.");
            return false;

        }
        bool FindUniqueInSubgrid()
        {
            int subgridWidth = _subgridWidth;
            List<CellValue> uniqueNumbers = new List<CellValue>(PossibleNumbers);
            foreach (Cell cell in RelatedCells.FindAll(c => c.Row / subgridWidth == Row / subgridWidth &&
                c.Column / subgridWidth == Column / subgridWidth))
                foreach (CellValue value in cell.PossibleNumbers)
                    uniqueNumbers.Remove(value);

            if (uniqueNumbers.Count == 1)
            {
                Value = uniqueNumbers[0];
                Solved = true;
                return true;
            }
            if (uniqueNumbers.Count > 1)
                throw new Exception("Row:" + Row + ",Column:" + Column + " more than 1 unique number.");
            return false;
        }
    }
}
