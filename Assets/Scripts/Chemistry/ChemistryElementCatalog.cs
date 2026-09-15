using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ChemistryElementCatalog
{
    public struct ElementInfo
    {
        public int AtomicNumber;
        public string Symbol;
        public string Name;
        public Color Color;

        public ElementInfo(int atomicNumber, string symbol, string name, Color color)
        {
            AtomicNumber = atomicNumber;
            Symbol = symbol;
            Name = name;
            Color = color;
        }
    }

    /// <summary>Hydrogen through Calcium (Z = 1–20), CPK-style colors.</summary>
    public static readonly ElementInfo[] FirstTwenty =
    {
        new ElementInfo(1, "H", "Hydrogen", new Color(0.95f, 0.95f, 0.98f)),
        new ElementInfo(2, "He", "Helium", new Color(0.85f, 1f, 1f)),
        new ElementInfo(3, "Li", "Lithium", new Color(0.80f, 0.50f, 1f)),
        new ElementInfo(4, "Be", "Beryllium", new Color(0.76f, 0.86f, 0.67f)),
        new ElementInfo(5, "B", "Boron", new Color(1f, 0.71f, 0.71f)),
        new ElementInfo(6, "C", "Carbon", new Color(0.28f, 0.28f, 0.30f)),
        new ElementInfo(7, "N", "Nitrogen", new Color(0.19f, 0.31f, 0.97f)),
        new ElementInfo(8, "O", "Oxygen", new Color(1f, 0.05f, 0.05f)),
        new ElementInfo(9, "F", "Fluorine", new Color(0.56f, 0.88f, 0.31f)),
        new ElementInfo(10, "Ne", "Neon", new Color(0.70f, 0.89f, 0.96f)),
        new ElementInfo(11, "Na", "Sodium", new Color(0.67f, 0.36f, 0.95f)),
        new ElementInfo(12, "Mg", "Magnesium", new Color(0.54f, 1f, 0.00f)),
        new ElementInfo(13, "Al", "Aluminum", new Color(0.75f, 0.65f, 0.65f)),
        new ElementInfo(14, "Si", "Silicon", new Color(0.94f, 0.78f, 0.63f)),
        new ElementInfo(15, "P", "Phosphorus", new Color(1f, 0.50f, 0.00f)),
        new ElementInfo(16, "S", "Sulfur", new Color(1f, 1f, 0.19f)),
        new ElementInfo(17, "Cl", "Chlorine", new Color(0.12f, 0.94f, 0.12f)),
        new ElementInfo(18, "Ar", "Argon", new Color(0.50f, 0.82f, 0.89f)),
        new ElementInfo(19, "K", "Potassium", new Color(0.56f, 0.25f, 0.83f)),
        new ElementInfo(20, "Ca", "Calcium", new Color(0.24f, 0.49f, 0.04f))
    };

    public static readonly string[] Symbols =
    {
        "H", "He", "Li", "Be", "B", "C", "N", "O", "F", "Ne",
        "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca",
        "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn",
        "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr",
        "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn",
        "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd",
        "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb",
        "Lu", "Hf", "Ta", "W", "Re", "Os", "Ir", "Pt", "Au", "Hg",
        "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th",
        "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm",
        "Md", "No", "Lr", "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds",
        "Rg", "Cn", "Nh", "Fl", "Mc", "Lv", "Ts", "Og"
    };

    static readonly HashSet<string> SymbolSet = new HashSet<string>(Symbols);
    static readonly Regex TokenRegex = new Regex(@"[A-Za-z]+", RegexOptions.Compiled);

    public static bool IsSymbol(string value)
    {
        return !string.IsNullOrEmpty(value) && SymbolSet.Contains(value);
    }

    public static string ParseSymbol(string objectName)
    {
        if (string.IsNullOrWhiteSpace(objectName))
            return null;

        var cleaned = objectName.Trim();
        if (IsSymbol(cleaned))
            return cleaned;

        foreach (Match match in TokenRegex.Matches(cleaned))
        {
            var token = match.Value;
            if (IsSymbol(token))
                return token;

            if (token.Length >= 2)
            {
                var two = char.ToUpperInvariant(token[0]) + token.Substring(1, 1).ToLowerInvariant();
                if (IsSymbol(two))
                    return two;
            }

            var one = char.ToUpperInvariant(token[0]).ToString();
            if (token.Length == 1 && IsSymbol(one))
                return one;
        }

        return null;
    }
}
