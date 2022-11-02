
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Linq;
using TMPro;
public class SepticVolGraph : MonoBehaviour {

    public static SepticVolGraph instance =null;   
    [SerializeField] private Sprite dotSprite;
    [SerializeField] private String LabelXaxis, LabelYaxis;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashContainer;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    private List<IGraphVisualObject> graphVisualObjectList;
    private GameObject tooltipGameObject;

    // Cached values
    public List<int> YvalueList, XvalueList;
    public List<float> XValueList_data, YValueList_data;

    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;
    IGraphVisual lineGraphVisual;
    IGraphVisual barChartVisual;
  //  [SerializeField]    TextMeshProUGUI[] Values;
    bool LineChart = false;
    [SerializeField] int YminVal, YmaxVal;
    [SerializeField] int  XmaxVal;
    public string XlabelName;
    [SerializeField] TMP_Dropdown _Dropdown;
    public float Barwidth;
    [SerializeField] TextMeshProUGUI ValueText;

    private void Awake() {
        instance = this;
        // Grab base objects references
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashContainer = graphContainer.Find("dashContainer").GetComponent<RectTransform>();
        dashTemplateX = dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        tooltipGameObject = graphContainer.Find("tooltip").gameObject;

        gameObjectList = new List<GameObject>();
        graphVisualObjectList = new List<IGraphVisualObject>();
        
        
        HideTooltip();


    }
    private void Start() {
        _Dropdown.value = 1;
        _DropDownVal();
    }
    public void _DropDownVal() {
        YValueList_data.Clear();
         lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f));
        barChartVisual = new BarChartVisual(graphContainer, Color.blue, Barwidth);
        if (_Dropdown.value == 0) {//per month
            XmaxVal = 4;
            Barwidth = .2f;
            XlabelName = "WEEK";
            YValueList_data = new List<float>(Graph.instance.Septic_Vol_YValueList_MonthlyData);
            ValueText.text = "550";

        }
        else if (_Dropdown.value == 1) {//per week
            XmaxVal = 7;
            Barwidth = .2f;
            XlabelName = "DAY";
            YValueList_data = new List<float>(Graph.instance.Septic_Vol_YValueList_WeeklyData);
            ValueText.text = "850";

        }

        _Graph();
    }
    public void FogGraph(int i) {
        if (YvalueList.Count < 12) {
            YvalueList.Add(i);           
        } else {
            YvalueList.RemoveAt(0);
            YvalueList.Add(i);          
        }
        if (LineChart) ShowGraph(YvalueList, lineGraphVisual, -1, (int _i) => (_i + 1) + "", (float _f) => Mathf.RoundToInt(_f) + "");
        else ShowGraph(YvalueList, barChartVisual, -1, (int _i) => (_i + 1) + "", (float _f) => Mathf.RoundToInt(_f) + "");

        //Values[0].text = ((int)valueList.Average()).ToString() + "ppm";
        //Values[1].text = valueList.Max().ToString() + "ppm";
        //Values[2].text = valueList.Min().ToString() + "ppm";
    }
    void _FoglevelGraph() {

    }
    public void _Graph() {
        YvalueList.Clear();
        XvalueList.Clear();
        for (int i = 0; i < YValueList_data.Count; i++) {         
            YvalueList.Add(Mathf.RoundToInt(YValueList_data[i]));
        }
        for (int i = 1; i <= YValueList_data.Count; i++) {
            XvalueList.Add(i);
        }
       
        // LineGraph();
        BarChart();
        
    }
    public void LineGraph() {
        //valueList.RemoveAt(0);
        // valueList.Add();
        SetGraphVisual(lineGraphVisual);
        LineChart = true;
    }
    public void BarChart() {
        // valueList[0]
        SetGraphVisual(barChartVisual);
        LineChart = false;

    }
    public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition) {
        instance.ShowTooltip(tooltipText, anchoredPosition);

    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPosition) {
        // Show Tooltip GameObject
      //  if(!HelmetSceneManager.instance.SimulationPanelAnim.GetCurrentAnimatorStateInfo(0).IsName("SimulationPanelOpen"))
            tooltipGameObject.SetActive(true);

        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        TextMeshProUGUI tooltipUIText = tooltipGameObject.transform.Find("text").GetComponent<TextMeshProUGUI>();
        tooltipUIText.text = tooltipText;

        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(
            tooltipUIText.preferredWidth + textPaddingSize * 2f,
            tooltipUIText.preferredHeight + textPaddingSize * 2f
        );

          tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        // UI Visibility Sorting based on Hierarchy, SetAsLastSibling in order to show up on top
        tooltipGameObject.transform.SetAsLastSibling();
    }

    public static void HideTooltip_Static() {
        instance.HideTooltip();
    }

    private void HideTooltip() {
        tooltipGameObject.SetActive(false);
    }


    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this.YvalueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }
    float yMaximum = 0;
    float yMinimum = 0;
    float xMaximum = 0;
    float xMinimum = 0;
    private void ShowGraph(List<int> YvalueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        this.graphVisual = graphVisual;
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;
        //if (maxVisibleValueAmount <= 0) {
        //    // Show all if no amount specified
        //    maxVisibleValueAmount = YvalueList.Count;
        //}
        //if (maxVisibleValueAmount > YvalueList.Count) {
        //    // Validate the amount to show the maximum
        //    maxVisibleValueAmount = YvalueList.Count;
        //}
        maxVisibleValueAmount = YvalueList.Count;
        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList) {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();

        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

       

       

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        if (YmaxVal == 0) {
            yMaximum = 10f;
            yMinimum = 1f; // Start the graph at zero 
            xMaximum = XmaxVal;
            xMinimum = 0f; // Start the graph at zero
        } else {
            yMaximum = YmaxVal;
            yMinimum = YminVal; // Start the graph at zero 
            xMaximum = XmaxVal;
            xMinimum = 0; // Start the graph at zero
        }
        // Set the distance between each point on the graph 
        float xSize = graphWidth / (maxVisibleValueAmount);



        // Set up separators on the x axis
        int separatorCount;
        separatorCount = XmaxVal;// valueList.Count;
        for (int i = 0; i <= separatorCount; i = i + 1) {
            // Duplicate the label template
            if (i != 0) {
                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer, false);
                labelX.gameObject.SetActive(true);
                float normalizedValue = i * 1f / separatorCount;
                labelX.anchoredPosition = new Vector2(normalizedValue * graphWidth, -55);
                //if (xMinimum + i == separatorCount/2) {
                //    labelX.transform.GetChild(0).GetComponent<Image>().sprite = ThickLine;
                //    labelX.transform.GetChild(0).GetComponent<Image>().transform.localScale = new Vector3(1.03f, 1f, 1f);

                //}
                labelX.GetComponent<TextMeshProUGUI>().text = XlabelName + " " + getAxisLabelX((int)(xMinimum + i));
                labelX.GetComponent<TextMeshProUGUI>().color = Color.white;
                gameObjectList.Add(labelX.gameObject);

              


            }
        }

        int separatorCount2 = 7, xIndex = 0;
        // Set up separators on the y axis
        for (int i = 0; i <= separatorCount2; i++) {
            // Duplicate the label template
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount2;
            labelY.anchoredPosition = new Vector2(-60f, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum))) + LabelYaxis;
            labelY.GetComponent<TextMeshProUGUI>().color = Color.black;
            //if (yMinimum + (normalizedValue * (yMaximum - yMinimum)) == 0) {
            //    labelY.transform.GetChild(0).GetComponent<Image>().sprite = ThickLine;
            //    labelY.transform.GetChild(0).GetComponent<Image>().transform.localScale = new Vector3(1.03f,1f,1f);
            //}
            gameObjectList.Add(labelY.gameObject);

            // Duplicate the dash template
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(dashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);



        }
        for (int i = Mathf.Max(YvalueList.Count - maxVisibleValueAmount, 0); i < YvalueList.Count; i++) {
            float xPosition = xSize + xIndex * xSize;
            // float xPosition = ((XvalueList[i] - xMinimum) / (xMaximum - xMinimum)) * graphWidth;
            float yPosition = ((YvalueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            string tooltipText = getAxisLabelX(XvalueList[i]) + LabelXaxis + "," + getAxisLabelY(YvalueList[i]) + LabelYaxis;
            // Debug.Log(tooltipText);
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipText);
            graphVisualObjectList.Add(graphVisualObject);

            // Duplicate the x dash template
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(dashContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -31f);
            gameObjectList.Add(dashX.gameObject);


            xIndex = xIndex + 1;
        }

    }

  

    /*
     * Interface definition for showing visual for a data point
     * */
    private interface IGraphVisual {

        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();
      
    }

    /*
     * Represents a single Visual Object in the graph
     * */
    private interface IGraphVisualObject {

        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();

       

        
    }

   
    /*
     * Displays data points as a Bar Chart
     * */
    
    private class BarChartVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Color barColor ;

        private float barWidthMultiplier;
        private int Xchecker;
        
        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier) {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

       

        public void CleanUp() {
            Xchecker = 0;
          
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);

            BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, barWidthMultiplier);
            barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
          

            return barChartVisualObject;
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth) {
           
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            if (SepticVolGraph.instance.YvalueList[Xchecker]%2==0) {
                gameObject.GetComponent<Image>().color = new Vector4(.18f,.65f,.98f, 1);  // Color.blue;               
            } else {
                gameObject.GetComponent<Image>().color = new Vector4(.69f, 1, .03f, 1); //Color.yellow;
            }
            // gameObject.GetComponent<Image>().color = _color;
            // gameObject.GetComponent<Image>().color = _color;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            
            // Add Button_UI Component which captures UI Mouse Events
            Button_UI barButtonUI = gameObject.AddComponent<Button_UI>();
            Xchecker++;

            return gameObject;
        }

       

        public class BarChartVisualObject : IGraphVisualObject {

            private GameObject barGameObject;
            private float barWidthMultiplier;

            public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier) {
                this.barGameObject = barGameObject;
                this.barWidthMultiplier = barWidthMultiplier;
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
                rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);

                Button_UI barButtonUI = barGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                barButtonUI.MouseOverOnceFunc = () => {
                    ShowTooltip_Static(tooltipText, graphPosition);
                };

                // Hide Tooltip on Mouse Out
                barButtonUI.MouseOutOnceFunc = () => {
                    HideTooltip_Static();
                };
            }

            public void CleanUp() {
                Destroy(barGameObject);
            }

          
        }

    }


    /*
     * Displays data points as a Line Graph
     * */
    private class LineGraphVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor) {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
           // this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastLineGraphVisualObject = null;
        }

        public void CleanUp() {
            lastLineGraphVisualObject = null;
        }


        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject dotGameObject = CreateDot(graphPosition);


            GameObject dotConnectionGameObject = null;
            if (lastLineGraphVisualObject != null) {
                dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            
            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, lastLineGraphVisualObject);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
            
            lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        public void SetBarColor(Color _Color) {
             this.dotColor = _Color;
        }

        private GameObject CreateDot(Vector2 anchoredPosition) {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.localScale = new Vector2(1.6f, 2.4f);

            // Add Button_UI Component which captures UI Mouse Events
            Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            gameObject.GetComponent<Image>().raycastTarget = false;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }


        public class LineGraphVisualObject : IGraphVisualObject {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject dotGameObject;
            private GameObject dotConnectionGameObject;
            private LineGraphVisualObject lastVisualObject;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, LineGraphVisualObject lastVisualObject) {
                this.dotGameObject = dotGameObject;
                this.dotConnectionGameObject = dotConnectionGameObject;
                this.lastVisualObject = lastVisualObject;

                if (lastVisualObject != null) {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();

                Button_UI dotButtonUI = dotGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                dotButtonUI.MouseOverOnceFunc = () => {
                    ShowTooltip_Static(tooltipText, graphPosition);
                };
            
                // Hide Tooltip on Mouse Out
                dotButtonUI.MouseOutOnceFunc = () => {
                    HideTooltip_Static();
                };

                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp() {
                Destroy(dotGameObject);
                Destroy(dotConnectionGameObject);
            }

            public Vector2 GetGraphPosition() {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection() {
                if (dotConnectionGameObject != null) {
                    RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }

        }

    }

}
