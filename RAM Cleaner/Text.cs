using System;

namespace RAM_Cleaner
{
    class Text
    {
        private string inputtxt;
        private string tableHeader;
        private string exit;
        private string tableH;
        private string tableFormat;
        private string title;
        private string table;
        private string inputAsk;

        public string Exit { get => exit; }
        public string Inputtxt { get => inputtxt; }
        public string TableHeader { get => tableHeader; }
        public string TableH { get => tableH; }
        public string TableFormat { get => tableFormat; }
        public string Title { get => title; }
        public string Table { get => table; set => table = value; }
        public string InputAsk { get => inputAsk; }

        public void Init()
        {
            inputtxt = ""
            + " \n" + "Cleaning options:"
            + " \n" + "1. Fast Clean"
            + " \n" + "2. Normal Clean"
            + " \n" + "3. Deep clean"
            + " \n \n";

            inputAsk = "Please press the number of the preferred option!";

            tableFormat = "|{0,10}|{1,10}|{2,10}|";

            tableHeader = string.Format(tableFormat, "Total RAM", "Free RAM", "Used RAM");

            exit = "--> Press Enter to restart! \n--> Press Esc to exit";

            tableH = "----------";

            title = "RAM Cleaner";


        }
        public string TableMaker(Int64 tot, Int64 phav, double percFree, double percUsed)
        {
            table = string.Format(tableFormat, tableH, tableH, tableH);
            table += " \n" + TableHeader;
            table += " \n" + string.Format(tableFormat, tableH, tableH, tableH);
            table += " \n" + string.Format(tableFormat, tot + " MB", phav + " MB", (tot - phav) + " MB");
            table += " \n" + string.Format(tableFormat, 100 + " %", percFree + " %", percUsed + " %");
            table += " \n" + string.Format(tableFormat, tableH, tableH, tableH);

            return table;
        }
    }
}
