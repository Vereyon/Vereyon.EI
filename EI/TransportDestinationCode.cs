using System;
using System.Collections.Generic;
using System.Resources;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    /// <summary>
    /// Based on Vektis code sheet COD101.
    /// </summary>
    public class TransportDestinationCode
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public static List<TransportDestinationCode> Codes { get; private set; }

        static TransportDestinationCode()
        {

            ResourceManager resourceManager;
            string data;

            // Obtain code list from resource.
            resourceManager = new ResourceManager(typeof(TransportDestinationCode));
            data = resourceManager.GetString("Data");
            Codes = new List<TransportDestinationCode>();

            // Add a default first choice.
            Codes.Add(new TransportDestinationCode { Id = -1, Name = "" });

            var lines = data.Split(new string [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {

                // Split the chunks at the tab. Skip any line not containing two chuncks.
                var chuncks = line.Split('\t');
                if (chuncks.Count() != 2)
                    continue;

                TransportDestinationCode code;

                code = new TransportDestinationCode
                {
                    Id = int.Parse(chuncks[0]),
                    Name = chuncks[1]
                };
                Codes.Add(code);
            }
        }
    }
}
