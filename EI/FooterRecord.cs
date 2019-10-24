using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    public class FooterRecord : EIRecord
    {

        public override int Code { get { return 99; } }

        public int InsuredPersonRecordCount { get; set; }
        public int DebtorRecordCount { get; set; }
        public int PerformanceRecordCount { get; set; }
        public int CommentRecordCount { get; set; }
        public int DetailRecordCount { get; set; }

        /// <summary>
        /// Gets / sets the total mount. Negative for credit.
        /// </summary>
        public double TotalAmount { get; set; }

        public FooterRecord()
        {

            MapField(2, 6, "Aantal verzekerdenrecords").Numeric().Getter(x => InsuredPersonRecordCount.ToString());
            MapField(8, 6, "Aantal debiteurrecords").Numeric().Getter(x => DebtorRecordCount.ToString());
            MapField(14, 6, "Aantal prestatierecords").Numeric().Getter(x => PerformanceRecordCount.ToString());
            MapField(20, 6, "Aantal commentaarrecords").Numeric().Getter(x => CommentRecordCount.ToString());
            MapField(26, 7, "Totaal aantal detailrecords").Numeric().Getter(x => DetailRecordCount.ToString());

            MapField(33, 11, "Totaal declaratiebedrag").Numeric().Getter(x => Math.Round(TotalAmount * 100).ToString());
            MapField(44, 1, "Indicatie debit/credit").Alphanumeric().Getter(x =>
            {
                if (TotalAmount >= 0)
                    return "D";
                else
                    return "C";
            });
        }
    }
}
