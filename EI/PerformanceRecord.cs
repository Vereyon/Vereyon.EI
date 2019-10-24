using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    public class TransportPerformanceRecord : EIRecord
    {

        public override int Code { get { return 04; } }

        private long _id;

        /// <summary>
        /// Gets / sets the unique record ID. This ID will be used in EI replies as well.
        /// Should start at 1 for the first record in the message.
        /// </summary>
        public long Id
        {
            get { return _id; }
            set
            {
                if (value >= 1000000000000)
                    throw new ArgumentOutOfRangeException("ID cannot be longer than 12 digits.");
                if (value < 0)
                    throw new ArgumentOutOfRangeException("ID cannot be negative.");
                _id = value;
            }
        }

        /// <summary>
        /// Identificatie van een (zorg)verzekeraar die betrokken is bij de uitvoering van de Basisverzekering en/of de Algemene Wet Bijzondere Ziektekosten.
        /// Consult UZOVI-register at Vektis http://uzovi.vektis.nl
        /// </summary>
        public int UzoviId { get; set; }

        public bool AllowForwarding { get; set; }

        /// <summary>
        /// COD101.
        /// </summary>
        public int TransportDestinationCode { get; set; }
        public long BurgerServiceNumber { get; set; }

        /// <summary>
        /// Indicates which performance code list is used. Lists define in COD367-VEKT.
        /// </summary>
        public int PerformanceCodeList { get; set; }

        public int PerformanceCode { get; set; }

        /// <summary>
        /// ID of the caregiver performing this treatment. First 2 digits specify the type of caregiver.
        /// Consult the registry at Vektis https://agb.vektis.nl/
        /// </summary>
        public int CareProviderAgbId { get; set; }

        public DateTime PerformanceDate { get; set; }

        /// <summary>
        /// Gets / sets the unit of the performance.
        /// COD132-VEKT
        /// </summary>
        public TransportMeasureUnit MeasureUnit { get; set; }

        public int UnitCount { get; set; }

        /// <summary>
        /// Gets / sets the unit price in cents including VAT.
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// Gets / sets the total amount of this performance including VAT. Negative if the amount is to be credited.
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Gets / sets the total amount invoiced including VAT. Negative if the amount is to be credited.
        /// </summary>
        public double InvoicedAmount { get; set; }

        /// <summary>
        /// Gets / sets if the preformance (treatment) declared was required due to an accident involving the patient. Set to null if unknown.
        /// </summary>
        public bool? DueToAccident { get; set; }

        public TransportPerformanceRecord()
        {

            MapField(2, 12, "Identificatie detailrecord").Numeric().Getter(x => Id.ToString());
            MapField(14, 9, "Burgerservicenummer").Numeric().Getter(x => BurgerServiceNumber.ToString());
            MapField(23, 4, "UZOVI-nummer").Numeric().Getter(x => UzoviId.ToString());

            MapField(57, 1, "Doorsturen toegestaan").Numeric().Getter(x =>
            {
                if (AllowForwarding)
                    return "1";
                else
                    return "0";
            });

            MapField(58, 1, "Indicatie ongeval").Alphanumeric().Getter(x => {
                if(!DueToAccident.HasValue)
                    return "O";
                else if(DueToAccident.Value)
                    return "J";
                else
                    return "N";
            });
            MapField(59, 4, "Code bestemming vervoer").Numeric().Getter(x => TransportDestinationCode.ToString());
            MapField(63, 3, "Aanduiding prestatiecodelijst").Numeric().Getter(x => PerformanceCodeList.ToString());
            MapField(66, 6, "Prestatiecode").Numeric().Getter(x => PerformanceCode.ToString());

            MapField(100, 8, "Zorgverlenerscode vervoerder").Numeric().Getter(x => CareProviderAgbId.ToString());
            MapField(108, 8, "Datum prestatie").Numeric().Getter(x => PerformanceDate.ToString("yyyyMMdd"));
            MapField(116, 4, "Vertrektijd vervoer").Numeric();

            MapField(144, 5, "Huisnummer herkomst").Numeric();
            MapField(220, 5, "Huisnummer bestemming").Numeric();

            MapField(272, 1, "Eenheid rit/prestatie").Numeric().Getter(x => MeasureUnit.ToString("D"));
            MapField(273, 4, "Aantal (rit)eenheden").Numeric().Getter(x => UnitCount.ToString());
            MapField(277, 8, "Tarief prestatie (incl. BTW)").Numeric().Getter(x => Math.Round(UnitPrice * 100).ToString());

            MapField(285, 8, "Bedrag toeslag").Numeric();

            MapField(293, 8, "Berekend bedrag (incl. BTW)").Numeric().Getter(x => Math.Round(TotalAmount * 100).ToString());
            MapField(301, 1, "Indicatie debet/credit").Alphanumeric().Getter(x =>
            {
                if (TotalAmount >= 0)
                    return "D";
                else
                    return "C";
            });

            MapField(302, 4, "BTW percentage declaratiebedrag").Numeric();
            MapField(306, 8, "Declaratiebedrag (incl. BTW)").Numeric().Getter(x => Math.Round(InvoicedAmount * 100).ToString());
            MapField(314, 1, "Indicatie debet/credit").Alphanumeric().Getter(x =>
            {
                if (InvoicedAmount >= 0)
                    return "D";
                else
                    return "C";
            });

            // Set default values.
            AllowForwarding = true;
            MeasureUnit = TransportMeasureUnit.Kilometer;
        }
    }

    /// <summary>
    /// Based on COD132-VEKT.
    /// </summary>
    public enum TransportMeasureUnit
    {
        /// <summary>
        /// Variable amount per unit. Not part of COD132-VEKT.
        /// </summary>
        Variable = 0,

        Kilometer = 1,
        Ride = 4
    }
}
