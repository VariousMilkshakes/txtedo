

namespace txtedo.modules
{
    class Web : Module.ModuleBase
    {
        public Web()
        {
            Init();
        }

        public override string[] Init()
        {
            Name = "web";
            Desc = "Used to search Interwebz";
            string[] info = {Name, Desc};

            AddCommand("Search", Search);

            return info;
        }

        private string[] Search(string input)
        {
            string[] Arr = new string[1];


            return Arr;
        }


        public override string[] OnRun()
        {
            throw new System.NotImplementedException();
        }
    }
}
