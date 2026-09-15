using TMPro;
using UnityEngine;

public class ElementUIManager : MonoBehaviour
{
    public TMP_Text atomicNumberText;
    public TMP_Text atomicMassText;
    public TMP_Text categoryText;
    public TMP_Text stateText;
    public TMP_Text meltingPointText;
    public TMP_Text boilingPointText;
    public TMP_Text descriptionText;

    public TMP_Text symbolText;
    public TMP_Text elementNameText;

    public AtomVisualizer atomVisualizer;

    private readonly string[] symbols =
    {
        "H","He","Li","Be","B","C","N","O","F","Ne",
        "Na","Mg","Al","Si","P","S","Cl","Ar",
        "K","Ca","Sc","Ti","V","Cr","Mn","Fe","Co","Ni",
        "Cu","Zn","Ga","Ge","As","Se","Br","Kr",
        "Rb","Sr","Y","Zr","Nb","Mo","Tc","Ru","Rh","Pd",
        "Ag","Cd","In","Sn","Sb","Te","I","Xe",
        "Cs","Ba","La","Ce","Pr","Nd","Pm","Sm","Eu","Gd",
        "Tb","Dy","Ho","Er","Tm","Yb","Lu",
        "Hf","Ta","W","Re","Os","Ir","Pt","Au","Hg",
        "Tl","Pb","Bi","Po","At","Rn",
        "Fr","Ra","Ac","Th","Pa","U","Np","Pu","Am","Cm",
        "Bk","Cf","Es","Fm","Md","No","Lr",
        "Rf","Db","Sg","Bh","Hs","Mt","Ds","Rg","Cn",
        "Nh","Fl","Mc","Lv","Ts","Og"
    };

    private readonly string[] names =
    {
        "Hydrogen","Helium","Lithium","Beryllium","Boron",
        "Carbon","Nitrogen","Oxygen","Fluorine","Neon",
        "Sodium","Magnesium","Aluminium","Silicon","Phosphorus",
        "Sulfur","Chlorine","Argon","Potassium","Calcium",
        "Scandium","Titanium","Vanadium","Chromium","Manganese",
        "Iron","Cobalt","Nickel","Copper","Zinc",
        "Gallium","Germanium","Arsenic","Selenium","Bromine",
        "Krypton","Rubidium","Strontium","Yttrium","Zirconium",
        "Niobium","Molybdenum","Technetium","Ruthenium","Rhodium",
        "Palladium","Silver","Cadmium","Indium","Tin",
        "Antimony","Tellurium","Iodine","Xenon","Cesium",
        "Barium","Lanthanum","Cerium","Praseodymium","Neodymium",
        "Promethium","Samarium","Europium","Gadolinium","Terbium",
        "Dysprosium","Holmium","Erbium","Thulium","Ytterbium",
        "Lutetium","Hafnium","Tantalum","Tungsten","Rhenium",
        "Osmium","Iridium","Platinum","Gold","Mercury",
        "Thallium","Lead","Bismuth","Polonium","Astatine",
        "Radon","Francium","Radium","Actinium","Thorium",
        "Protactinium","Uranium","Neptunium","Plutonium","Americium",
        "Curium","Berkelium","Californium","Einsteinium","Fermium",
        "Mendelevium","Nobelium","Lawrencium","Rutherfordium","Dubnium",
        "Seaborgium","Bohrium","Hassium", "Meitnerium","Darmstadtium",
"Roentgenium","Copernicium","Nihonium","Flerovium",
"Moscovium","Livermorium","Tennessine","Oganesson"
};
    private readonly int[] atomicNumber =
{
    1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
    11, 12, 13, 14, 15, 16, 17, 18,
    19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
    31, 32, 33, 34, 35, 36,
    37, 38, 39, 40, 41, 42, 43, 44, 45, 46,
    47, 48, 49, 50, 51, 52, 53, 54,
    55, 56, 57, 58, 59, 60, 61, 62, 63, 64,
    65, 66, 67, 68, 69, 70, 71,
    72, 73, 74, 75, 76, 77, 78, 79, 80,
    81, 82, 83, 84, 85, 86,
    87, 88, 89, 90, 91, 92, 93, 94, 95, 96,
    97, 98, 99, 100, 101, 102, 103,
    104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118};
    private readonly float[] atomicMass =
{
    1.008f, 4.0026f, 6.94f, 9.0122f, 10.81f, 12.011f, 14.007f, 15.999f, 18.998f, 20.180f,
    22.990f, 24.305f, 26.982f, 28.085f, 30.974f, 32.06f, 35.45f, 39.948f,
    39.098f, 40.078f, 44.956f, 47.867f, 50.942f, 51.996f, 54.938f, 55.845f, 58.933f, 58.693f, 63.546f, 65.38f,
    69.723f, 72.630f, 74.922f, 78.971f, 79.904f, 83.798f,
    85.468f, 87.62f, 88.906f, 91.224f, 92.906f, 95.95f, 98f, 101.07f, 102.91f, 106.42f,
    107.87f, 112.41f, 114.82f, 118.71f, 121.76f, 127.60f, 126.90f, 131.29f,
    132.91f, 137.33f, 138.91f, 140.12f, 140.91f, 144.24f, 145f, 150.36f, 151.96f, 157.25f,
    158.93f, 162.50f, 164.93f, 167.26f, 168.93f, 173.05f, 174.97f,
    178.49f, 180.95f, 183.84f, 186.21f, 190.23f, 192.22f, 195.08f, 196.97f, 200.59f,
    204.38f, 207.2f, 208.98f, 209f, 210f, 222f,
    223f, 226f, 227f, 232.04f, 231.04f, 238.03f, 237f, 244f, 243f, 247f,
    247f, 251f, 252f, 257f, 258f, 259f, 262f,
    267f, 268f, 269f, 270f, 277f, 278f, 281f, 282f, 285f, 286f,
    289f, 290f, 293f, 294f, 294f, 295f
}; private readonly string[] category =
{
    "Nonmetal", "Noble Gas", "Alkali Metal", "Alkaline Earth Metal", "Metalloid", "Nonmetal", "Nonmetal", "Nonmetal", "Halogen", "Noble Gas",
    "Alkali Metal", "Alkaline Earth Metal", "Post-transition Metal", "Metalloid", "Nonmetal", "Nonmetal", "Halogen", "Noble Gas",
    "Alkali Metal", "Alkaline Earth Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal",
    "Post-transition Metal", "Metalloid", "Metalloid", "Nonmetal", "Halogen", "Noble Gas",
    "Alkali Metal", "Alkaline Earth Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal",
    "Transition Metal", "Transition Metal", "Post-transition Metal", "Post-transition Metal", "Metalloid", "Metalloid", "Halogen", "Noble Gas",
    "Alkali Metal", "Alkaline Earth Metal", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide",
    "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide", "Lanthanide",
    "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal",
    "Post-transition Metal", "Post-transition Metal", "Post-transition Metal", "Metalloid", "Halogen", "Noble Gas",
    "Alkali Metal", "Alkaline Earth Metal", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide",
    "Actinide", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide", "Actinide",
    "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Transition Metal", "Unknown", "Transition Metal", "Transition Metal", "Transition Metal", "Post-transition Metal",
    "Post-transition Metal", "Post-transition Metal", "Post-transition Metal", "Post-transition Metal", "Post-transition Metal", "Post-transition Metal", "Post-transition Metal"
}; private readonly string[] state =
{
    "Gas", "Gas", "Solid", "Solid", "Solid", "Solid", "Gas", "Gas", "Gas", "Gas",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Gas", "Gas",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Liquid", "Gas",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Gas",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Liquid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Gas",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
    "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid"
}; private readonly string[] meltingPoint =
{
    "-259.16 °C", "-272.20 °C", "180.54 °C", "1287 °C", "2076 °C", "3550 °C", "-210.00 °C", "-218.79 °C", "-219.67 °C", "-248.59 °C",
    "97.79 °C", "650 °C", "660.32 °C", "1414 °C", "44.15 °C", "115.21 °C", "-101.50 °C", "-189.34 °C",
    "63.38 °C", "842 °C", "1541 °C", "1668 °C", "1910 °C", "1857 °C", "1246 °C", "1538 °C", "1495 °C", "1455 °C", "1084.62 °C", "419.53 °C",
    "29.76 °C", "938.25 °C", "817 °C", "221 °C", "-7.20 °C", "-157.36 °C",
    "39.31 °C", "777 °C", "1526 °C", "1855 °C", "2477 °C", "2623 °C", "2157 °C", "2334 °C", "1964 °C", "1554.90 °C",
    "961.78 °C", "321.07 °C", "156.60 °C", "231.93 °C", "630.63 °C", "449.51 °C", "113.70 °C", "-111.75 °C",
    "28.44 °C", "727 °C", "920 °C", "798 °C", "931 °C", "1024 °C", "1042 °C", "1072 °C", "826 °C", "1312 °C",
    "1356 °C", "1412 °C", "1474 °C", "1529 °C", "1545 °C", "824 °C", "1652 °C",
    "2227 °C", "3017 °C", "3422 °C", "3186 °C", "3033 °C", "2446 °C", "2041 °C", "1768 °C", "1064.18 °C",
    "-38.83 °C", "304 °C", "271.40 °C", "254 °C", "302 °C", "202 °C",
    "27 °C", "700 °C", "1050 °C", "1750 °C", "1572 °C", "1132 °C", "639 °C", "640 °C", "994 °C", "1340 °C",
    "986 °C", "900 °C", "860 °C", "1527 °C", "827 °C", "826 °C", "1627 °C",
    "2100 °C", "2200 °C", "2150 °C", "1300 °C", "1000 °C", "830 °C", "770 °C", "650 °C", "650 °C", "700 °C",
    "700 °C", "700 °C", "162 °C", "140 °C", "64 °C", "35 °C", "30 °C", "20 °C"
}; private readonly string[] boilingPoint =
{
    "-252.87 °C", "-268.93 °C", "1342 °C", "2469 °C", "3927 °C", "4027 °C", "-195.79 °C", "-182.95 °C", "-188.11 °C", "-246.08 °C",
    "883 °C", "1090 °C", "2519 °C", "3265 °C", "280.5 °C", "444.72 °C", "-34.04 °C", "-185.85 °C",
    "759 °C", "1484 °C", "2836 °C", "3287 °C", "3407 °C", "2670 °C", "2061 °C", "2862 °C", "2927 °C", "2913 °C", "2562 °C", "907 °C",
    "2403 °C", "2820 °C", "614 °C", "685 °C", "58.8 °C", "-153.42 °C",
    "688 °C", "1382 °C", "3338 °C", "4409 °C", "4744 °C", "4639 °C", "4265 °C", "4150 °C", "3695 °C", "2963 °C",
    "2162 °C", "767 °C", "2072 °C", "2602 °C", "1587 °C", "988 °C", "184.25 °C", "-108.12 °C",
    "671 °C", "1845 °C", "3464 °C", "3443 °C", "3520 °C", "3074 °C", "1794 °C", "3235 °C", "1597 °C", "3273 °C",
    "3230 °C", "2567 °C", "2700 °C", "2868 °C", "1950 °C", "1196 °C", "3402 °C",
    "4603 °C", "5458 °C", "5555 °C", "5596 °C", "5012 °C", "4428 °C", "3825 °C", "3825 °C", "2807 °C",
    "356.73 °C", "1473 °C", "1564 °C", "962 °C", "337 °C", "-61.7 °C",
    "677 °C", "1140 °C", "3200 °C", "4788 °C", "4000 °C", "4131 °C", "3902 °C", "3228 °C", "2607 °C", "3110 °C",
    "2027 °C", "2690 °C", "1740 °C", "1267 °C", "1627 °C", "1470 °C", "1173 °C",
    "4200 °C", "4300 °C", "4300 °C", "2900 °C", "2800 °C", "2600 °C", "2400 °C", "2000 °C", "1600 °C", "1300 °C",
    "1200 °C", "1100 °C", "145 °C", "140 °C", "147 °C", "170 °C", "120 °C", "115 °C"
}; private readonly string[] description =
{
    "The lightest element and the most abundant element in the universe.",
    "A colorless, odorless noble gas used in balloons and cryogenics.",
    "A soft, silvery alkali metal and the lightest solid element.",
    "A hard, lightweight alkaline earth metal used in alloys and aerospace applications.",
    "A metalloid used in glass, ceramics, detergents, and semiconductors.",
    "A nonmetal that forms the basis of organic chemistry and life.",
    "A colorless gas essential for proteins and living organisms.",
    "A reactive gas essential for respiration and combustion.",
    "A highly reactive halogen used in dental care and chemical manufacturing.",
    "An inert noble gas commonly used in lighting and signs.",
    "A soft, reactive alkali metal commonly found in salts and biological systems.",
    "A lightweight alkaline earth metal important in biological and industrial applications.",
    "A lightweight metal widely used in aircraft, packaging, and construction.",
    "A metalloid widely used in electronics, glass, and semiconductor technology.",
    "A reactive nonmetal essential for DNA, ATP, and living organisms.",
    "A reactive nonmetal essential for life and widely used in industry.",
    "A reactive halogen commonly used in disinfectants and chemical production.",
    "An inert noble gas used in lighting, welding, and industrial processes.",
    "A soft alkali metal important in fertilizers and biological processes.",
    "An alkaline earth metal important in bones, minerals, and construction materials.",
    "A transition metal used in alloys, aerospace materials, and industrial applications.",
    "A strong transition metal widely used in aerospace alloys and pigments.",
    "A transition metal used to strengthen steel and produce specialized alloys.",
    "A hard transition metal widely used in stainless steel and industrial alloys.",
    "A transition metal used in steel production and many industrial applications.",
    "A strong transition metal widely used in steel and construction.",
    "A transition metal used in high-strength alloys and magnets.",
    "A transition metal used in stainless steel, coins, and industrial alloys.",
    "A transition metal widely used in electrical wiring and electronics.",
    "A transition metal used for galvanizing steel and protecting it from corrosion.",
    "A soft metal used in semiconductors, LEDs, and specialized alloys.",
    "A metalloid widely used in semiconductor and optical applications.",
    "A toxic metalloid used in semiconductors, alloys, and specialized applications.",
    "A nonmetal used in glassmaking, pigments, and chemical compounds.",
    "A reactive halogen used in flame retardants and chemical manufacturing.",
    "A noble gas used in lighting, lasers, and cryogenic applications.",
    "A soft alkali metal used in research, electronics, and specialized applications.",
    "An alkaline earth metal used in alloys, fireworks, and medical imaging.",
    "A transition metal used in alloys and high-strength materials.",
    "A transition metal used in ceramics, nuclear technology, and strong alloys.",
    "A transition metal used in superconducting materials and specialized alloys.",
    "A transition metal used in steel alloys, catalysts, and industrial applications.",
    "A radioactive transition metal used mainly in scientific research.",
    "A transition metal used in catalysts, electronics, and chemical applications.",
    "A transition metal used in catalysts and specialized industrial applications.",
    "A transition metal widely used in catalytic converters and electronics.",
    "A precious transition metal used in jewelry, electronics, and catalysts.",
    "A transition metal used for corrosion-resistant coatings and batteries.",
    "A soft precious metal widely used in jewelry, electronics, and photography.",
    "A soft metal used in batteries, coatings, and specialized alloys.",
    "A post-transition metal used in semiconductors and specialized materials.",
    "A soft metal widely used in solder, coatings, and alloys.",
    "A metalloid used in flame retardants, alloys, and semiconductor materials.",
    "A metalloid used in electronics, solar cells, and specialized materials.",
    "A halogen essential to thyroid function and widely used in medicine.",
    "A noble gas used in lighting, ion propulsion, and scientific research.",
    "A highly reactive alkali metal used mainly in research.",
    "An alkaline earth metal used in drilling fluids, fireworks, and medical imaging.",
    "A lanthanide used in specialized glass, catalysts, and research.",
    "A lanthanide used in catalysts, glassmaking, and scientific applications.",
    "A lanthanide used in magnets, lasers, and specialized materials.",
    "A lanthanide used in magnets, lasers, and advanced materials.",
    "A radioactive lanthanide used mainly in scientific research.",
    "A lanthanide used in magnets, lasers, and specialized materials.",
    "A lanthanide used in lasers, phosphors, and specialized materials.",
    "A lanthanide used in magnets, lasers, and electronic applications.",
    "A lanthanide used in phosphors, magnets, and specialized materials.",
    "A lanthanide used in lasers, magnets, and electronic applications.",
    "A lanthanide used in magnets, lasers, and specialized materials.",
    "A lanthanide used in lasers and specialized optical materials.",
    "A lanthanide used in magnets and specialized electronic materials.",
    "A lanthanide used in fiber optics, lasers, and specialized alloys.",
    "A lanthanide used in lasers, magnets, and optical applications.",
    "A transition metal used in high-temperature alloys and industrial applications.",
    "A strong transition metal used in electronics, aerospace, and specialized alloys.",
    "A transition metal with a very high melting point used in electronics and alloys.",
    "A transition metal used in high-temperature alloys and catalysts.",
    "A dense transition metal used in electrical contacts and specialized alloys.",
    "A precious transition metal used in electronics, jewelry, and catalysts.",
    "A precious transition metal widely used in jewelry, electronics, and catalysts.",
    "A precious metal used in jewelry, electronics, and corrosion-resistant applications.",
    "A heavy liquid metal historically used in thermometers and industrial applications.",
    "A soft post-transition metal used in electronics and specialized alloys.",
    "A dense metal used in batteries, radiation shielding, and construction.",
    "A heavy post-transition metal used mainly in scientific research.",
    "A radioactive element used mainly in scientific research.",
    "A radioactive halogen used mainly in scientific research.",
    "A radioactive noble gas produced naturally from radioactive decay.",
    "A highly radioactive alkali metal found only in trace amounts.",
    "A radioactive alkaline earth metal used in research and radiation applications.",
    "A radioactive actinide used mainly in scientific research.",
    "A radioactive actinide used in nuclear research and fuel studies.",
    "A radioactive actinide used mainly in nuclear research.",
    "A radioactive actinide widely known for its role in nuclear energy and weapons.",
    "A radioactive actinide used in nuclear reactors and scientific research.",
    "A radioactive actinide used mainly in research and specialized applications.",
    "A radioactive actinide used in nuclear research and scientific applications.",
    "A radioactive actinide used mainly for scientific research.",
    "A radioactive actinide used mainly in research.",
    "A radioactive actinide used mainly in scientific research.",
    "A radioactive actinide used mainly in research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research.",
    "A radioactive synthetic element used mainly in scientific research."
};
    public void ShowElement(string symbol)
    {
        for (int i = 0; i < symbols.Length; i++)
        {
            if (symbols[i] == symbol)
            {
                ShowElement(i + 1);
                return;
            }
        }

        Debug.LogWarning("Unknown element: " + symbol);
    }

    public void ShowElement(int number)
    {
        if (number < 1 || number > 118)
        {
            Debug.LogWarning("Invalid atomic number: " + number);
            return;
        }

        int index = number - 1;

        if (atomicNumberText != null)
            atomicNumberText.text = "Atomic Number: " + atomicNumber[index];

        if (atomicMassText != null)
            atomicMassText.text = "Atomic Mass: " + atomicMass[index].ToString("0.###") + " u";

        if (categoryText != null)
            categoryText.text = "Category: " + category[index];

        if (stateText != null)
            stateText.text = "State: " + state[index];

        if (meltingPointText != null)
            meltingPointText.text = "Melting Point: " + meltingPoint[index];

        if (boilingPointText != null)
            boilingPointText.text = "Boiling Point: " + boilingPoint[index];

        if (descriptionText != null)
            descriptionText.text = "Description: " + description[index];

        if (symbolText != null)
            symbolText.text = symbols[index];

        if (elementNameText != null)
            elementNameText.text = names[index].ToUpper();

        if (atomVisualizer != null)
        {
            int protons = atomicNumber[index];
            int neutrons = Mathf.Max(
                0,
                Mathf.RoundToInt(atomicMass[index]) - protons
            );
            int electrons = protons;

            atomVisualizer.BuildElement(
                protons,
                neutrons,
                electrons
            );
        }

        Debug.Log(
            "Selected: " +
            symbols[index] +
            " - " +
            names[index]
        );
    }
}