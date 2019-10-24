using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vereyon.Vecozo.EI;

namespace Vereyon.Vecozo.EI
{
    /// <summary>
    /// The EIWriter writer Externe Integratie (EI) standard records to a TextWriter.
    /// </summary>
    public class EIWriter
    {

        /// <summary>
        /// Gets / sets the length of the records being written. Records in a single message have a common fixed length.
        /// </summary>
        public int RecordLength { get; private set; }

        public TextWriter Writer { get; private set; }

        public EIWriter(TextWriter writer, int recordLength)
        {

            Writer = writer;
            RecordLength = recordLength;
        }

        public void Serialize(EIRecord record)
        {
            record.Length = RecordLength;
            record.Serialize(Writer);
        }
    }
}
