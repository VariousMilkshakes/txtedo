namespace txtedo.modules
{
    class Web : Module.ModuleBase
    {
        public override string[] Init()
        {
            //Apply settings for module
            Name = "web";
            Desc = "Used to search Interwebz";
            hasQuery = true;
            string[] info = {Name, Desc};

            //Create child modules
            Seach SearchModule = new Seach();
            //Add child modules
            AddCommand(SearchModule);

            //Return information about module. May not be needed.
            return info;
        }

        private class Seach : Module.ModuleBase
        {
            public override string[] Init()
            {
                Child();

                Name = "Search";
                Desc = "Seach Google";
                hasQuery = true;
                string[] info = { Name, Desc };

                return info;
            }

            public override void OnRun(string input)
            {
                System.Console.WriteLine("RUN CHILD MODULE");
                System.Console.WriteLine(input);
            }
        }


        public override void OnRun(string input)
        {
            System.Console.WriteLine("RUN MODULE");
            System.Console.WriteLine(input);
        }
    }
}
