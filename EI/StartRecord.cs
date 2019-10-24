using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    /// <summary>
    /// VOORLOOPRECORD
    /// </summary>
    public class StartRecord : EIRecord
    {

        public int StandardVersion { get; set; }
        public int StandardSubversion { get; set; }

        public override int Code { get { return 1; } }

        public int MessageCode { get; set; }

        /// <summary>
        /// Gets / sets the type of this message. 'P' for production, 'T' for test.
        /// </summary>
        public MessageType Type { get; set; }

        public PaymentRecipient PaymentRecipientCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// ISO currency code. Possible values defined in code list COD363.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Identificatie van een (zorg)verzekeraar die betrokken is bij de uitvoering van de Basisverzekering en/of de Algemene Wet Bijzondere Ziektekosten.
        /// Consult UZOVI-register at Vektis http://uzovi.vektis.nl
        /// </summary>
        public int UzoviId { get; set; }

        /// <summary>
        /// ID of the caregiver performing this treatment. First 2 digits specify the type of caregiver.
        /// Consult the registry at Vektis https://agb.vektis.nl/
        /// </summary>
        public int CareProviderAgbId { get; set; }

        public StartRecord()
        {

            // Set default values.
            StandardVersion = 4;
            StandardSubversion = 2;
            MessageCode = 117;

            // Make this a test message by default.
            Type = MessageType.Test;

            InvoiceId = string.Empty;
            CurrencyCode = "EUR";

            // Define the fields for this record.
            MapField(2, 3, "Code").Numeric().Getter(x => MessageCode.ToString("000"));
            MapField(5, 2, "Versienummer berichtstandaard").Numeric().Getter(x => StandardVersion.ToString());
            MapField(7, 2, "Subversienummer berichtstandaard").Numeric().Getter(x => StandardSubversion.ToString());
            MapField(9, 1, "Soortbericht").Numeric().Getter(x =>
            {
                switch (Type)
                {
                    case MessageType.Production:
                        return "P";
                    case MessageType.Test:
                        return "T";
                    default:
                        throw new InvalidOperationException("Message type is not specified.");
                }
            });

            MapField(10, 6, "Code informatiesysteem softwareleverancier").Numeric();

            MapField(26, 4, "UZOVI-nummer").Numeric().Getter(x => UzoviId.ToString());
            MapField(30, 8, "Code servicebureau").Numeric();
            MapField(38, 8, "Zorgverlenerscode").Numeric().Getter(x => CareProviderAgbId.ToString());
            MapField(46, 8, "Praktijkcode").Numeric();
            MapField(54, 8, "Instellingcode").Numeric();

            MapField(62, 2, "Identificatiecode betaling aan").Numeric().Getter(x => PaymentRecipientCode.ToString("D"));
            MapField(64, 8, "Begindatum declaratieperiode").Numeric().Getter(x => StartDate.ToString("yyyyMMdd"));
            MapField(72, 8, "Einddatum declaratieperiode").Numeric().Getter(x => EndDate.ToString("yyyyMMdd"));

            MapField(80, 12, "Factuurnummer declarant").Alphanumeric().Getter(x => InvoiceId);
            MapField(92, 8, "Dagtekening factuur").Numeric().Getter(x => InvoiceDate.ToString("yyyyMMdd"));

            MapField(114, 3, "Valutacode").Alphanumeric().Getter(x => CurrencyCode);

            
        }
    }

    /// <summary>
    /// Based on code list COD833.
    /// </summary>
    public enum PaymentRecipient : byte
    {

        /// <summary>
        /// A service party forms an electronic barrier between the care provider and the insurer, clearing and factoring invoices.
        /// </summary>
        ServiceParty = 01,

        /// <summary>
        /// Zorgverlener. Indentification of a natural person.
        /// </summary>
        CareProvider = 02,

        /// <summary>
        /// Praktijk. Identification of practice to which care provider belongs.
        /// </summary>
        CarePractice = 03,

        /// <summary>
        /// Zorginstelling
        /// </summary>
        CareFacility = 04,
    }

    public enum MessageType
    {
        Test,
        Production
    }
}
