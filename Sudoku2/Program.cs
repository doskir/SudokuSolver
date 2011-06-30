using System;
using System.IO;
using System.Text;

namespace Sudoku
{
    class Program
    {
        //The gridstring is the sudoku grid read from left to right and top to bottom
        //Using 0 as the replacement for an empty cell
        //Example:
        /* 
╔═════╦═════╦═════╗
║ │ │ ║ │ │3║ │7│2║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║ │ │5║ │ │4║ │ │3║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║ │ │ ║2│8│ ║ │ │9║
╠═════╬═════╬═════╣
║7│1│ ║ │ │ ║6│ │ ║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║8│ │4║ │ │ ║9│ │1║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║ │ │2║ │ │ ║ │4│7║
╠═════╬═════╬═════╣
║9│ │ ║ │1│2║ │ │ ║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║6│ │ ║5│ │ ║2│ │ ║
╠─┼─┼─║─┼─┼─║─┼─┼─╣
║2│5│ ║9│ │ ║ │ │ ║
╚═════╩═════╩═════╝
         *Converts to the gridstring 000003072005004003000280009710000600804000901002000047900012000600500200250900000
         */

        const string TestString1 =
    "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
        const string TestString2 =
            "004050900005800104000100000040002003600405008100900050000001000409007500001020700";
        const string TestString3 =
            "000004270600085000007120000002000090001306700040000300000079100000450002054600000";
        const string TestString4 =
            "000003072005004003000280009710000600804000901002000047900012000600500200250900000";

        const string TestString5 =
            "JP00000000LA000B000E0D00KBM0F0J00NK0BCL0J0000A0G00F0I0MOG00H0L00GKJB0000000F00LDA0P0000LBKE0000N000000A00H0000I00I0000N0ADJ00KG00CE00AKH0M0000N00J0000P00N000000K0000NDIH0000M0FMA00C0000000LEHK00A0N00PMI0D0F00I0L0000E0FAK0GD00F0N0OIC00B0J000P000HL00000000BA";

        private const string TestString6 =
            "0BN000PD000A00E000I00J0A00000D000000MF000N00OL0PFEJ0HB000L000A0GI0EDA00B0000L0G00000K00M0C00000A0000P000MJOIN0D0J00K0DHO0000E0M00K0A0000JGD0C00F0G0BNPFJ000K0000P00000A0L00N00000D0J0000O00BPE0KK0L000N000MG0IFBH0GI00K000NJ000000D00000K0B00H000F00L000IH000NA0";

        private const string TestString7 =
            "00L000U00K000G0000CQ00I0X0000QH0R00K00000V000AMT00DABHI000C000XT00F000000VKUVXW00DJE000IRA00O00CH000K00G0XF0BWQD00L0000000O0000000QIU000O0S0000R0000PN000E0DVK0J000PQ00L0AMIY0H00F0C0RA0000NUG000IWS00B00I0QH0OY00L00D00B00KT0U000MG00WC00000T000000J0R0DVM0R0EB000000D00C000H0A0GW000PW00G00AY00E00000X0N00000KY00L00000WUT0ND000000HLTS0000W000V0KX000F00QJ0NB0000H0SYFP0CIK0E0L000TOLQ00000E0UR0F00BI00XD0J0TB00A00000C0000YS00UVP00X0T00XP0J00000QA000W0ERY0C00H00NYQ000XISE0F00T00KWA000KYFTA00G00PN0000JOQL00UVW000U0P000H00RG0X000NM0Q0G0O00T00F0X0BNM00KC00E0YCU0J0GYNOS000I0V0F00B0000I0000RE000WSO0J00A0000K000P00TC0XMD000Q00000Y0ULO0";

        private const string TestString8 =
            "00LF00G0R0EX0JA00D0000000B0S0M000J00000000RE00000A0000000V0000C0B00NL0PHY000P000BW00MLV000UOSC0000N0W0X00000YHD0PSU00G000CL0I0D0B0000000J0XLQP000E000000I00FY000H0EC000000T00RSO0JK000RLS0000000W00G00D00000000N0V00RPT0FKM0000000V0Y00AQEDG00N0HCU00K000F0TMO000000I00B0Y0AH00E0U00A0000PJ0G00DT00I00N0X0Q0YBE00000M00000V00000SAH0000GU00L0BQ000RFP00O00M0J0HJRLPI000E0000S00Q0U0O00V0H00O0C0F0U00G0RNE0A00000000S0A0PK000JVYO000000U000CA0U00D0000B0M0XY00N00F00000000O0LN0FQX0000000M000EWPD0MXGB0I00OC0V000000H00YTB0S0000A0000UC00L0000PL0E0KU0W00O0000SI00000YJ0000ITVGD0C0NYR0K00000A0OX0F0VH0YN0JK0I0TAP0G0W00MNMUA00E000F0V0WJ000B00RGK";

        static void Main(string[] args)
        {
            Console.WindowHeight = 50;            
            Console.WindowWidth = 85;
            Console.WriteLine(
    "A gridstring looks like 530070000600195000098000060800060003400803001700020006060000280000419005000080079");
            Console.WriteLine("Substitute 0 for empty fields and do not use line-breaks.");
            while (true)
            {
                Console.Write("Enter a gridstring: ");
                string inputString = Console.ReadLine();
                if (inputString != null && inputString.Length < 5)
                {
                    switch (inputString)
                    {
                        case "1":
                            inputString = TestString1;
                            break;
                        case "2":
                            inputString = TestString2;
                            break;
                        case "3":
                            inputString = TestString3;
                            break;
                        case "4":
                            inputString = TestString4;
                            break;
                        case "5":
                            inputString = TestString5;
                            break;
                        case "6":
                            inputString = TestString6;
                            break;
                        case "7":
                            inputString = TestString7;
                            break;
                        case "8":
                            inputString = TestString8;
                            break;
                        default:
                            Console.WriteLine("Invalid Gridstring.");
                            break;
                    }
                }
                try
                {
                    Grid grid = new Grid(inputString);
                    if (grid.Cells == null)
                        continue;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Original");
                    DrawGrid(grid);

                    if (grid.Solve())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Solved");
                        DrawGrid(grid);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unsolved");
                        DrawGrid(grid);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static void DrawGrid(Grid grid)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            int subgridWidth = (int) Math.Sqrt(grid.SideLength);

            Console.Write("╔");
            for (int i = 0; i < subgridWidth -1;i++)
                Console.Write(new string('═', subgridWidth * 2 - 1) + "╦");
            Console.WriteLine(new string('═', subgridWidth*2 - 1) + "╗");
              
            
            //Console.WriteLine("╔═════╦═════╦═════╗");
            for (int row = 0; row < grid.SideLength; row++)
            {                
                for (int column = 0; column < grid.SideLength; column++)
                {
                    Cell cell = grid.Cells[row, column];
                    if (column % subgridWidth == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("║");
                        if (cell.Value == Cell.CellValue.None)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("░");
                        }
                        else
                        {
                            if (grid.SolvedCells.Contains(cell))
                                Console.ForegroundColor = ConsoleColor.Green;
                            else if (grid.PresolvedCells.Contains(cell))
                                Console.ForegroundColor = ConsoleColor.Gray;
                            if (grid.SideLength > 9 && cell.Value != Cell.CellValue.None)
                                Console.Write((char)('@' + cell.Value));
                            else
                                Console.Write((int) cell.Value);
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("│");
                        if (cell.Value == Cell.CellValue.None)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("░");
                        }
                        else
                        {
                            if (grid.SolvedCells.Contains(cell))
                                Console.ForegroundColor = ConsoleColor.Green;
                            else if (grid.PresolvedCells.Contains(cell))
                                Console.ForegroundColor = ConsoleColor.Gray;
                            if (grid.SideLength > 9)
                                Console.Write((char)('@' + cell.Value));
                            else
                                Console.Write((int)cell.Value);
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("║");
                if ((row + 1) % subgridWidth == 0 && row < grid.SideLength - 1)
                {
                    Console.Write("╠");
                    for (int i = 0; i < subgridWidth - 1; i++)
                        Console.Write(new string('═', subgridWidth * 2 - 1) + "╬");
                    Console.WriteLine(new string('═', subgridWidth * 2 - 1) + "╣");
                }
                else if (row != grid.SideLength -1)
                {
                    Console.Write("╠");
                    for (int i = 0; i < subgridWidth ; i++)
                    {
                        for (int j = 0; j < subgridWidth -1;j++)
                        {
                            Console.Write("─┼");
                        }
                        Console.Write("-║");
                    }
                    Console.WriteLine();
                    
                    //Console.WriteLine("╠─┼─┼─║─┼─┼─║─┼─┼─╣");
                }
            }
            Console.Write("╚");
            for (int i = 0; i < subgridWidth - 1; i++)
                Console.Write(new string('═', subgridWidth * 2 - 1) + "╩");
            Console.WriteLine(new string('═', subgridWidth * 2 - 1) + "╝");
            Console.WriteLine(grid.SolvedCells.Count +  " cells solved.");
            Console.WriteLine(grid.GridString);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
