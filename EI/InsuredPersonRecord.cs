using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    public class InsuredPersonRecord : EIRecord
    {

        public override int Code { get { return 02; } }

        private long _id;

        /// <summary>
        /// Gets / sets the unique record ID. This ID will be used in EI replies as well.
        /// Should start at 1 for the first record in the message.
        /// </summary>
        public long Id { get { return _id; }
            set
            {
                if(value >= 1000000000000)
                    throw new ArgumentOutOfRangeException("ID cannot be longer than 12 digits.");
                if (value < 0)
                    throw new ArgumentOutOfRangeException("ID cannot be negative.");
                _id = value;
            }
        }

        public long BurgerServiceNumber { get; set; }

        /// <summary>
        /// The surname of the insured person. Will be abbreviated if too long.
        /// </summary>
        public string SurName { get; set; }
        public string SurnamePrefix { get; set; }
        public string Initials { get; set; }

        public int HouseNumber { get; set; }

        /// <summary>
        /// Alpha 2 country code (ISO 3166) of the address of the insured person. Leave empty if in Netherlands. Enter "00" in case of a currently non existing historic nation.
        /// </summary>
        public string AddressCountryCode { get; set; }

        public string Postcode { get; set; }

        public bool Deceased { get; set; }

        /// <summary>
        /// Identificatie van een (zorg)verzekeraar die betrokken is bij de uitvoering van de Basisverzekering en/of de Algemene Wet Bijzondere Ziektekosten.
        /// Consult UZOVI-register at Vektis http://uzovi.vektis.nl
        /// </summary>
        public int UzoviId { get; set; }

        /// <summary>
        /// Gets / sets the customer/relation ID of this person at the insurance company. Known as the 'VERZEKERDENNUMMER' in Dutch.
        /// </summary>
        public string RelationId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        //public int NameCode { get; set; }

        public InsuredPersonRecord()
        {

            // Set default values.
            //NameCode = 1;
            AddressCountryCode = "00";
            Postcode = string.Empty;
            RelationId = string.Empty;

            // Declare fields.
            MapField(2, 12, "Identificatie detailrecord").Numeric().Getter(x => Id.ToString());
            MapField(14, 9, "Burgerservicenummer").Numeric().Getter(x => BurgerServiceNumber.ToString());
            MapField(23, 4, "UZOVI-nummer").Numeric().Getter(x => UzoviId.ToString());
            MapField(27, 15, "Verzekerdennummer").Alphanumeric().Getter(x => RelationId);

            MapField(53, 8, "Datum geboort verzekerde").Numeric().Getter(x => DateOfBirth.ToString("yyyyMMdd"));
            MapField(61, 1, "Geslacht verzekerde").Numeric().Getter(x =>
            {
                if (Gender == EI.Gender.Male)
                    return "1";
                else
                    return "2";
            });
            MapField(62, 1, "Naamcode/naamgebruik (01)").Numeric().Getter(x => "1");
            MapField(63, 25, "Naam verzekerde").Alphanumeric().Getter(x => SurName).SetTrim(EIFieldTrim.End);
            MapField(88, 10, "Voorvoegsel verzekerde").Alphanumeric().Getter(x => SurnamePrefix).SetTrim(EIFieldTrim.End);

            //  Second name of insured person only for use by service companies.
            MapField(98, 1, "Naamcode/naamgebruik (02)").Numeric().Getter(x => "0");

            MapField(134, 6, "Voorletters verzekerde").Alphanumeric().Getter(x => Initials).SetTrim(EIFieldTrim.End);

            MapField(140, 1, "Naamcode/naamgebruik (03)").Numeric().Getter(x => "1");

            MapField(141, 6, "Postcode (huisadres) verzekerde").Alphanumeric().Getter(x => {
                if(AddressCountryCode == "00" || AddressCountryCode == "NL")
                    return Postcode;
                else
                    return string.Empty;
            });
            MapField(147, 9, "Postcode buitenland").Alphanumeric().Getter(x =>
            {
                if (AddressCountryCode == "00" || AddressCountryCode == "NL")
                    return string.Empty;
                else
                    return Postcode;

            });
            MapField(167, 2, "Code land verzekerde").Alphanumeric().Getter(x => AddressCountryCode);

            MapField(156, 5, "Huisnummer verzekerde").Numeric().Getter(x => HouseNumber.ToString());
            MapField(180, 1, "Indicatie client overleden").Numeric().Getter(x => {
                if (Deceased)
                    return "1";
                else
                    return "2";
            });
        }
    }

    public enum Gender
    {
        Male = (int)'M',
        Female = (int)'F'
    }
}
