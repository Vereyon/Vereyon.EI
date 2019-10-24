using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vereyon.Vecozo.EI
{
    public class EIField
    {

        public int Offset { get; set; }
        public int Length { get; set; }

        public string Name { get; set; }

        public EIFieldType Type { get; set; }
        public EIFieldTrim Trim { get; set; }

        private EIFieldSetter _setter;
        private EIFieldGetter _getter;
        private string _value;

        public EIField()
        {

            Name = string.Empty;
            Type = EIFieldType.Alphanumeric;
            Trim = EIFieldTrim.None;
            _value = string.Empty;
        }

        public void Set(string value)
        {
            _value = value;
            if (_setter != null)
                _setter(this, value);
        }

        public string Get()
        {
            if (_getter != null)
                return _getter(this);
            return _value;
        }

        public EIField SetName(string name)
        {

            Name = name;
            return this;
        }

        public EIField Numeric()
        {

            Type = EIFieldType.Numeric;
            return this;
        }

        public EIField Alphanumeric()
        {

            Type = EIFieldType.Alphanumeric;
            return this;
        }

        public EIField Setter(EIFieldSetter setter)
        {
            _setter = setter;
            return this;
        }

        public EIField Getter(EIFieldGetter getter)
        {
            _getter = getter;
            return this;
        }

        public EIField SetTrim(EIFieldTrim trim)
        {
            Trim = trim;
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return "Unnamed field";
            return Name;
        }
    }

    public enum EIFieldType
    {
        /// <summary>
        /// Een waarde in een numeriek veld wordt rechts uitgelijnd en zo nodig voorzien van voorloopnullen, tenzik anders vermeld.
        /// Een numeriek veld heeft een vaste lengte.
        /// Een numeriek veld heeft als escapewaarde “nullen”.
        /// Een numeriek veld heeft “specifieke afspraken” voor het hanteren van een dummywaarde.
        /// </summary>
        Numeric,

        /// <summary>
        /// Een waarde in een alfanumeriek veld wordt links uitgelijnd en zo nodig rechts aangevuld met spaties, tenzij anders vermeld.
        /// Een alfanumeriek veld heeft een vaste lengte (ook al is dit niet altijd zo beschreven);
        /// Een alfanumeriek veld heeft als escapewaarde “spaties”.
        /// Een alfanumeriek veld heeft “specifieke afspraken” voor het hanteren van een dummywaarde.
        /// </summary>
        Alphanumeric
    }

    /// <summary>
    /// Indicates if a field value which is to long should be trimmed and if so, how.
    /// </summary>
    public enum EIFieldTrim
    {
        None = 0,
        Start = 1,
        End = 2
    }

    public delegate void EIFieldSetter(EIField field, string value);
    public delegate string EIFieldGetter(EIField field);
}
