using UnityEngine;

public class PeriodicTableBinder: MonoBehaviour
{
    public ElementUIManager elementUIManager;
    public ElementModeManager elementModeManager;

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

    private Transform[] elementObjects;


    private void Start()
    {
        elementObjects = new Transform[symbols.Length];

        for (int i = 0; i < symbols.Length; i++)
        {
            GameObject element =
                GameObject.Find(symbols[i]);

            if (element != null)
            {
                elementObjects[i] =
                    element.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Element not found: " +
                    symbols[i]
                );
            }
        }
    }


    private void Update()
    {
        // ==========================================
        // فقط الماوس
        // ==========================================

        if (!Input.GetMouseButtonDown(0))
            return;


        // ==========================================
        // لا تسمح بالضغط على العناصر
        // إلا في Information Mode
        // ==========================================

        if (elementModeManager == null)
        {
            Debug.LogError(
                "ElementModeManager is not assigned!"
            );

            return;
        }


        if (!elementModeManager.informationMode)
        {
            Debug.Log(
                "Mouse clicked element but Information Mode is OFF."
            );

            return;
        }


        if (Camera.main == null)
            return;


        // ==========================================
        // Raycast
        // ==========================================

        Ray ray =
            Camera.main.ScreenPointToRay(
                Input.mousePosition
            );

        RaycastHit hit;


        if (!Physics.Raycast(ray, out hit))
            return;


        Transform closestElement =
            FindClosestElement(hit.point);


        if (closestElement == null)
        {
            Debug.LogWarning(
                "Could not find element for click."
            );

            return;
        }


        string symbol =
            closestElement.name;


        Debug.Log(
            "MOUSE CLICKED ELEMENT: " +
            symbol
        );


        if (elementUIManager != null)
        {
            elementUIManager.ShowElement(symbol);
        }
    }


    private Transform FindClosestElement(
        Vector3 hitPoint)
    {
        Transform closest = null;

        float closestDistance =
            Mathf.Infinity;


        for (int i = 0;
             i < elementObjects.Length;
             i++)
        {
            if (elementObjects[i] == null)
                continue;


            float distance =
                Vector3.Distance(
                    hitPoint,
                    elementObjects[i].position
                );


            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = elementObjects[i];
            }
        }


        return closest;
    }
}