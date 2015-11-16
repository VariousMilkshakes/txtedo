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

            Seach SearchModule = new Seach();

            AddCommand(SearchModule);

            return info;
        }

        private class Seach : Module.ModuleBase
        {
            public Seach()
            {
                Init();
            }

            public override string[] Init()
            {
                Name = "Search";
                Desc = "Seach Google";
                string[] info = { Name, Desc };

                return info;
            }

            public override string[] OnRun()
            {
                throw new System.NotImplementedException();
            }
        }


        public override string[] OnRun()
        {
            throw new System.NotImplementedException();
        }
    }
}
