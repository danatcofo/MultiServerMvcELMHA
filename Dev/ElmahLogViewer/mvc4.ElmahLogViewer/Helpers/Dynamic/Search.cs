using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.Search
{
    public class search : List<IsLogical>
    {
        public search() : base() { }

        public search(IEnumerable<IsLogical> collection) : base(collection) { }

        public search(int capacity) : base(capacity) { }

        public override string ToString()
        {
            GetParams();

            string str = string.Empty;
            foreach (object o in this)
                str += string.IsNullOrWhiteSpace(str) ?
                    str = o.ToString() :
                    (((o is Or) ? " OR " : " AND ") + o.ToString());
            return string.IsNullOrWhiteSpace(str) ? "true" : str;
        }

        public object[] GetParams()
        {
            List<Const> parms = new List<Const>();
            List<Const> dparms = new List<Const>();
            this.Where(a => a != null).ToList().ForEach(a => parms.AddRange(a.GetParams()));
            dparms.AddRange(parms.Distinct());
            int i = 0;
            dparms.ForEach(a => { a.Index = i; ++i; });
            parms.ForEach(a =>
            {
                a.Index = dparms.FirstOrDefault(b => b.Equals(a)).Index;
            });
            return (from a in dparms
                    select a.v).ToArray();
        }
    }

    public class sort : List<IsSortable>
    {
        public sort() : base() { }

        public sort(IEnumerable<IsSortable> collection) : base(collection) { }

        public sort(int capacity) : base(capacity) { }

        public override string ToString() { return string.Join(", ", this); }
    }

    public class Asc : Get, IsSortable
    {
        public Asc() : base() { }

        public Asc(string value) : base(value) { }

        public override string ToString() { return string.Format("{0} ASC", v); }
    }

    public class Desc : Get, IsSortable
    {
        public Desc() : base() { }

        public Desc(string value) : base(value) { }

        public override string ToString() { return string.Format("{0} DESC", v); }
    }

    #region Interfaces

    /// <summary>
    /// Denotes an object that is Sortable
    /// </summary>
    public interface IsSortable { }

    /// <summary>
    /// Denotes objects that can be grouped into a list.
    /// </summary>
    public interface IsGroupable { IEnumerable<Const> GetParams(); }

    /// <summary>
    /// Denotes objects that can be transformed into a Boolean evaluation.
    /// </summary>
    public interface IsLogical { IEnumerable<Const> GetParams(); }

    /// <summary>
    /// Denotes objects that are added, subtracted, multiplied, divided and modulated.
    /// </summary>
    public interface IsArithmetic { IEnumerable<Const> GetParams(); }

    public interface IsInverted { object O { get; } }

    public interface IsSignature { object L { get; } object R { get; } }

    #endregion Interfaces

    #region Groupings

    public class And : List<IsLogical>, IsLogical
    {
        public And() : base() { }

        public And(IEnumerable<IsLogical> collection) : base(collection) { }

        public And(int capacity) : base(capacity) { }

        public override string ToString()
        {
            return string.Format("({0})", string.Join(" AND ", this.Where(a => a != null)));
        }

        public IEnumerable<Const> GetParams()
        {
            var list = new List<Const>();

            this.Where(a => a != null).ToList().ForEach(a => list.AddRange(a.GetParams()));

            return list;
        }
    }

    public class Or : List<IsLogical>, IsLogical
    {
        public Or() : base() { }

        public Or(IEnumerable<IsLogical> collection) : base(collection) { }

        public Or(int capacity) : base(capacity) { }

        public override string ToString()
        {
            return string.Format("({0})", string.Join(" OR ", this.Where(a => a != null)));
        }

        public IEnumerable<Const> GetParams()
        {
            var list = new List<Const>();

            this.Where(a => a != null).ToList().ForEach(a => list.AddRange(a.GetParams()));

            return list;
        }
    }

    #endregion Groupings

    #region Signatures

    public abstract class LogicalSignature : IsSignature, IsLogical
    {
        public LogicalSignature() { }

        public LogicalSignature(IsLogical left, IsLogical right)
        {
            l = left;
            r = right;
        }

        public object L { get { return l; } }

        public object R { get { return r; } }

        protected abstract string operation { get; }

        public IsLogical l { get; set; }

        public IsLogical r { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", l, operation, r);
        }

        public IEnumerable<Const> GetParams()
        {
            var list = new List<Const>();

            if (l != null) list.AddRange(l.GetParams());
            if (r != null) list.AddRange(r.GetParams());

            return list;
        }
    }

    public abstract class MathSignature : IsSignature, IsArithmetic
    {
        public MathSignature() { }

        public MathSignature(IsArithmetic left, IsArithmetic right)
        {
            l = left;
            r = right;
        }

        public object L { get { return l; } }

        public object R { get { return r; } }

        protected abstract string operation { get; }

        public IsArithmetic l { get; set; }

        public IsArithmetic r { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", l, operation, r);
        }

        public IEnumerable<Const> GetParams()
        {
            var list = new List<Const>();

            if (l != null) list.AddRange(l.GetParams());
            if (r != null) list.AddRange(r.GetParams());

            return list;
        }
    }

    #endregion Signatures

    #region Inversion

    /// <summary>
    /// !(o)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Not<T> : IsInverted, IsGroupable, IsLogical where T : IsLogical
    {
        public Not(T obj) { o = obj; }

        public object O { get { return o; } }

        public T o { get; set; }

        public override string ToString()
        {
            return string.Format("!({0})", o);
        }

        public IEnumerable<Const> GetParams()
        {
            if (o != null) return o.GetParams();
            else return new List<Const>();
        }
    }

    /// <summary>
    /// Negates the results of the value
    /// </summary>
    public class Negative<T> : IsInverted, IsArithmetic where T : IsArithmetic
    {
        public Negative(T obj) { o = obj; }

        public object O { get { return o; } }

        public T o { get; set; }

        public override string ToString()
        {
            return string.Format("(-({0}))", o);
        }

        public IEnumerable<Const> GetParams()
        {
            if (o != null) return o.GetParams();
            else return new List<Const>();
        }
    }

    #endregion Inversion

    #region End Points

    /// <summary>
    /// Represents a Getter
    /// </summary>
    public class Get : IsLogical, IsArithmetic
    {
        public Get() : base() { }

        public Get(string value) : this() { v = value; }

        private string _v;

        public string v
        {
            get { return _v; }
            set
            {
                Contract.Assert(!string.IsNullOrWhiteSpace(value));
                _v = value;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(v)) throw new NullReferenceException("Found an empty string.");
            string value = v.Trim();

            string str = string.Empty;
            if (!"_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(value[0]))
                str += "@";
            // TODO: parse for keywords and prepend the @ symbol
            return str + value;
        }

        public IEnumerable<Const> GetParams()
        {
            return new List<Const>();
        }
    }

    /// <summary>
    /// Represents a Constant expression
    /// </summary>
    public class Const : IsLogical, IsArithmetic
    {
        public Const() : base() { }

        public Const(object value) : this() { v = value; }

        public Const(object value, int index) : this(value) { this.Index = index; }

        private int _Index = -1;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        private object _v;

        public object v
        {
            get { return _v; }
            set
            {
                Contract.Assert(
                    value is Boolean ||
                    value is Char || value is String ||
                    value is Guid ||
                    value is SByte || value is Byte ||
                    value is Int32 || value is UInt32 || value is Int16 || value is UInt16 || value is Int64 || value is UInt64 || value is Enum ||
                    value is Single || value is Double || value is Decimal ||
                    value is DateTime ||
                    value is TimeSpan || value == null
                    );
                _v = value;
            }
        }

        public override string ToString()
        {
            return Index >= 0 ?
                "@" + Index.ToString() :
                v.Parse();
        }

        public IEnumerable<Const> GetParams()
        {
            return new List<Const> { this, };
        }

        public override int GetHashCode()
        {
            return v.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return v.Equals((obj is Const) ? ((Const)obj).v : obj);
        }
    }

    public class ConstList : List<Const>, IsLogical
    {
        public ConstList() : base() { }

        public ConstList(IEnumerable<Const> collection) : base(collection) { }

        public ConstList(int capacity) : base(capacity) { }

        public IEnumerable<Const> GetParams() { return this; }

        public override string ToString() { return "{0} = " + string.Join("OR {0} = ", this); }
    }

    #endregion End Points

    #region Mathmatical Operations

    /// <summary>
    /// x + y
    /// <para>l + r</para>
    /// </summary>
    public class Add : MathSignature, IsArithmetic
    {
        public Add() : base() { }

        public Add(IsArithmetic x, IsArithmetic y) : base(x, y) { }

        protected override string operation
        {
            get { return " + "; }
        }
    }

    /// <summary>
    /// x - y
    /// <para>l - r</para>
    /// </summary>
    public class Sub : MathSignature
    {
        public Sub() : base() { }

        public Sub(IsArithmetic x, IsArithmetic y) : base(x, y) { }

        protected override string operation
        {
            get { return " - "; }
        }
    }

    /// <summary>
    /// a / b
    /// <para>l / r</para>
    /// </summary>
    public class Div : MathSignature
    {
        public Div() : base() { }

        public Div(IsArithmetic a, IsArithmetic b) : base(a, b) { }

        protected override string operation
        {
            get { return " / "; }
        }
    }

    /// <summary>
    /// x * y
    /// <para>l * r</para>
    /// </summary>
    public class Mult : MathSignature
    {
        public Mult() : base() { }

        public Mult(IsArithmetic x, IsArithmetic y) : base(x, y) { }

        protected override string operation
        {
            get { return " * "; }
        }
    }

    /// <summary>
    /// a mod n
    /// <para>a % n</para>
    /// <para>l % r</para>
    /// </summary>
    public class Mod : MathSignature
    {
        public Mod() : base() { }

        public Mod(IsArithmetic a, IsArithmetic n) : base(a, n) { }

        protected override string operation
        {
            get { return " % "; }
        }
    }

    #endregion Mathmatical Operations

    #region Equivalators

    /// <summary>
    /// g in b
    /// </summary>
    public class IN : IsSignature, IsLogical
    {
        public IN() : base() { }

        public IN(Get g, ConstList i) { l = g; r = i; }

        public Get l { get; set; }

        public ConstList r { get; set; }

        public override string ToString() { return string.Format("(" + R.ToString() + ")", L); }

        public object L { get { return l; } }

        public object R { get { return r; } }

        public IEnumerable<Const> GetParams()
        {
            if (r == null) return new List<Const>();
            else return r.GetParams();
        }
    }

    /// <summary>
    /// a == b
    /// <para>l == r</para>
    /// </summary>
    public class EE : LogicalSignature
    {
        public EE() : base() { }

        public EE(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " == "; }
        }
    }

    /// <summary>
    /// a != b
    /// <para>l != r</para>
    /// </summary>
    public class NE : LogicalSignature
    {
        public NE() : base() { }

        public NE(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " != "; }
        }
    }

    /// <summary>
    /// a > b
    /// <para>l > r</para>
    /// </summary>
    public class GT : LogicalSignature
    {
        public GT() : base() { }

        public GT(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " > "; }
        }
    }

    /// <summary>
    /// a &lt; b
    /// <para>l &lt; r</para>
    /// </summary>
    public class LT : LogicalSignature
    {
        public LT() : base() { }

        public LT(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " < "; }
        }
    }

    /// <summary>
    /// a >= b
    /// <para>l >= r</para>
    /// </summary>
    public class GTE : LogicalSignature
    {
        public GTE() : base() { }

        public GTE(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " >= "; }
        }
    }

    /// <summary>
    /// a &lt;= b
    /// <para>l &lt;= r</para>
    /// </summary>
    public class LTE : LogicalSignature
    {
        public LTE() : base() { }

        public LTE(IsLogical a, IsLogical b) : base(a, b) { }

        protected override string operation
        {
            get { return " <= "; }
        }
    }

    #endregion Equivalators

    public static class HelperExtensions
    {
        internal static string Parse(this object v)
        {
            if (v == null) return "null";
            else if (v is string
                || v is Guid
                || v is DateTime
                || v is TimeSpan
                ) return string.Format("\"{0}\"", v);
            else if (v is char) return string.Format("'{0}'", v);
            else if (v is bool) return v.ToString().ToLower();
            return v.ToString();
        }

        public static search ToSearch(this Dictionary<string, object> dictionary)
        {
            if (dictionary == null || dictionary.Keys.Count == 0) return null;
            return new search(
                from i in dictionary.Keys
                select new EE(new Get(i), new Const(dictionary[i]))
                );
        }

        public static sort ToSort(this Dictionary<string, object> dictionary)
        {
            if (dictionary == null || dictionary.Keys.Count == 0) return null;
            sort sort = new sort();
            foreach (var s in dictionary.Keys)
            {
                var o = dictionary[s];
                if (o == null ||
                    (o is string || o is Enumerable) &&
                    (o.ToString().ToLower() == "ascending" ||
                    o.ToString().ToLower() == "asc"))
                    sort.Add(new Asc(s));
                else if ((o is string || o is Enumerable) &&
                    (o.ToString().ToLower() == "descending" ||
                    o.ToString().ToLower() == "desc"))
                    sort.Add(new Desc(s));
                else throw new InvalidCastException("Unable to parse the sort object.");
            }
            return sort;
        }

        public static void LocalizeSearch(this IEnumerable<IsLogical> search, IDictionary<string, string> map, IEnumerable<string> unsupported)
        {
            if (search == null) return;
            foreach (IsLogical i in search)
                if (i is IEnumerable<IsLogical>)
                    ((IEnumerable<IsLogical>)i).LocalizeSearch(map, unsupported);
                else if (i is IsSignature)
                    LocalizeSearch((IsSignature)i, map, unsupported);
                else LocalizeSearch(i, map, unsupported);
        }

        private static void LocalizeSearch(IsSignature signature, IDictionary<string, string> map, IEnumerable<string> unsupported)
        {
            if (signature == null) return;
            LocalizeSearch(signature.L, map, unsupported);
            LocalizeSearch(signature.R, map, unsupported);
        }

        private static void LocalizeSearch(object p, IDictionary<string, string> map, IEnumerable<string> unsupported)
        {
            if (p == null) return;
            if (p is IEnumerable<IsLogical>)
                ((IEnumerable<IsLogical>)p).LocalizeSearch(map, unsupported);
            else if (p is IsSignature)
                LocalizeSearch((IsSignature)p, map, unsupported);
            else if (p is Get)
            {
                Get v = (Get)p;
                if (map != null && map.ContainsKey(v.v.ToLower()))
                    v.v = map[v.v.ToLower()];
                if (unsupported != null && unsupported.Contains(v.v.ToLower()))
                    throw new InvalidOperationException("search contains the unsupported term {" + v.v + "]");
            }
        }
    }
}