namespace txtedo.Network
{
    public class Packet
    {
        private string sessionID;
        private string sessionSecret;
        private int packageCode;
        private JsonObject packageBody;

        public Packet() { }

        public Packet(string i, string sec, int cod, JsonObject pack)
        {
            this.sessionID = i;
            this.sessionSecret = sec;
            this.packageCode = cod;
            this.packageBody = pack;
        }

        //Write Only? Maybe
        public string id
        {
            //get { return this.sessionID; }
            set { this.sessionID = value; }
        }

        public string secret
        {
            //get { return this.sessionSecret; }
            set { this.sessionSecret = value; }
        }

        public int code
        {
            //get { return this.packageCode; }
            set { this.packageCode = value; }
        }

        public JsonObject body
        {
            //get { return this.packageBody; }
            set { this.packageBody = value; }
        }

        public string getJsonString()
        {
            JsonObject json = new JsonObject();
            json.Add("sessionID", this.sessionID);
            json.Add("sessionSecret", this.sessionSecret);
            json.Add("code", this.packageCode);
            json.Add("package", this.packageBody);

            return json.ToString();
        }
    }
}
