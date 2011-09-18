using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1_Pankov_Currency
{
    public class Currency : IComparable<Currency>, IEquatable<Currency>
    {
        public decimal Amount { get; private set; }
        public Symbols Symbol { get; private set; }

        public Currency(Symbols sym, decimal amount)
        {
            Symbol = sym;
            Amount = amount;
        }

        /// <summary>
        /// Coverts this currency into another currency
        /// </summary>
        /// <param name="to">Destination currency</param>
        /// <returns>New currency</returns>
        public Currency Convert(Symbols to)
        {
            return new Currency(to, ((to == Symbol) ? 1 : Market.GetConversionRatio(Symbol, to)) * Amount);
        }

        #region Implementations
        public override bool Equals(object obj)
        {
            if (!(obj is Currency)) return false;
            return CompareTo((Currency)obj) == 0;
        }

        public int CompareTo(Currency other)
        {
            return Amount.CompareTo(other.Amount);
        }

        public bool Equals(Currency other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() + Symbol.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0} {1:0.00}", Symbol, Amount);
        }
        #endregion

        #region Math
        public static Currency operator +(Currency a, Currency b)
        {
            return new Currency(a.Symbol, a.Amount + b.Convert(a.Symbol).Amount);
        }

        public static Currency operator +(Currency a, decimal b)
        {
            return new Currency(a.Symbol, a.Amount + b);
        }

        public static Currency operator -(Currency a, decimal b)
        {
            return new Currency(a.Symbol, a.Amount - b);
        }

        public static Currency operator -(Currency a, Currency b)
        {
            return new Currency(a.Symbol, a.Amount - b.Convert(a.Symbol).Amount);
        }

        public static Currency operator *(Currency a, decimal b)
        {
            return new Currency(a.Symbol, a.Amount * b);
        }

        public static Currency operator /(Currency a, decimal b)
        {
            return new Currency(a.Symbol, a.Amount / b);
        }

        public static bool operator ==(Currency a, Currency b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Currency a, Currency b)
        {
            return !(a == b);
        }
        #endregion
    }
}
