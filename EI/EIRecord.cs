using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    /// <summary>
    /// The EIRecord class is an abstract representation of an EI record. A concrete implementation
    /// needs to fill EI field specifications on the EI record.
    /// </summary>
    public abstract class EIRecord
    {

        public IList<EIField> Fields { get; set; }

        /// <summary>
        /// Gets / sets the total length of the EI record in characters according to specifications.
        /// </summary>
        public int Length { get; set; }

        public abstract int Code { get; }

        public EIRecord()
        {

            Fields = new List<EIField>();

            MapField(0, 2, "Kenmerk record").Numeric().Getter(x => Code.ToString("00"));
        }

        public EIField MapField(int offset, int length)
        {

            EIField field;

            field = new EIField();
            field.Offset = offset;
            field.Length = length;

            Fields.Add(field);

            return field;
        }

        public EIField MapField(int offset, int length, string name)
        {
            return MapField(offset, length).SetName(name);
        }

        public void Serialize(TextWriter writer)
        {

            int offset = 0;

            // TODO: Cache this for better performance?
            var fields = Fields.OrderBy(x => x.Offset);

            foreach (var field in fields)
            {

                // Fill the skipped space with spaces.
                while(offset < field.Offset)
                {
                    writer.Write(' ');
                    offset++;
                }

                // Get the field value.
                var value = field.Get();
                int deficit = field.Length - value.Length;
                if (deficit < 0)
                {

                    // The value is too long, decide how to act based on the specified trim behaviour.
                    switch (field.Trim)
                    {
                        case EIFieldTrim.None:
                            throw new Exception(string.Format("Value length exceeds maximum length for field {0}.", field.Name));
                        case EIFieldTrim.End:
                            value = value.Substring(0, value.Length + deficit);
                            break;
                        case EIFieldTrim.Start:
                            value = value.Substring(-deficit);
                            break;
                    }
                }

                // Based on field type, apply the correct padding.
                switch (field.Type)
                {
                    case EIFieldType.Numeric:

                        // Prefix with zeros.
                        while (deficit > 0)
                        {
                            writer.Write('0');
                            deficit--;
                        }
                        writer.Write(value);

                        break;
                    case EIFieldType.Alphanumeric:

                        // Append spaces.
                        writer.Write(value);
                        while (deficit > 0)
                        {
                            writer.Write(' ');
                            deficit--;
                        }
                        break;
                    default:
                        throw new Exception("Unknown field type.");
                }

                // Set the offset before moving to next field.
                offset = field.Offset + field.Length;
            }

            // Pad the message until the desired message length is attained.
            while (offset < Length)
            {
                writer.Write(' ');
                offset++;
            }

            // Close the message with a CR LF.
            writer.Write("\r\n");
        }

        public void Deserialize()
        {
        }
    }
}
