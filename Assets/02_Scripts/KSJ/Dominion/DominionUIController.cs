using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore;

public class DominionUIController : MonoBehaviour
{
    private ClickDominionObjs clickDominionObj;

    [Header("기본 정보")]
    [SerializeField] private Transform placeCameraPoint;

    [SerializeField] private PlaceInformation currPlaceInfo;
    [SerializeField] private string uiPlaceText;
    [SerializeField] public TextMeshProUGUI uiPlaceTextTmp;

    [Header("생산")]
    [SerializeField] private Button produceToggleButton;
    [SerializeField] private TextMeshProUGUI produceToggleText;

    [Header("어비스")]
    [SerializeField] private Button enterAbyssButton;

    [Header("상점")]
    [SerializeField] private Button enterShop;


    [SerializeField] private Button collectProduceButton;
    [SerializeField] private bool isProducing;


    private void Awake()
    {
        clickDominionObj = gameObject.GetComponent<ClickDominionObjs>();
    }

    private void OnEnable()
    {
        clickDominionObj.OnUIChanged += InitUI;
    }

    public void InitUI(PlaceInformation placeInfo)
    {
        currPlaceInfo = placeInfo;
        uiPlaceText = placeInfo.PlaceName;
        uiPlaceTextTmp.text = uiPlaceText;

        isProducing = placeInfo.IsProducing;

        // 생산 시작/중지 버튼 갱신
        ColorBlock color = produceToggleButton.colors;
        color.normalColor = isProducing ? Color.red : Color.white;
        color.selectedColor = isProducing ? Color.red : Color.white;
        produceToggleButton.colors = color;

        produceToggleText.text = isProducing ? "생산 중지" : "생산 시작";



        if (!currPlaceInfo.gameObject.CompareTag("ProducePlace"))
        {
            enterAbyssButton.gameObject.SetActive(false);
            produceToggleButton.gameObject.SetActive(false);
            collectProduceButton.gameObject.SetActive(false);
            enterShop.gameObject.SetActive(false);

            if (currPlaceInfo.gameObject.CompareTag("Abyss"))
            {
                produceToggleButton.gameObject.SetActive(false);
                collectProduceButton.gameObject.SetActive(false);
                enterShop.gameObject.SetActive(false);

                enterAbyssButton.gameObject.SetActive(true);
            }

            else if(currPlaceInfo.gameObject.CompareTag("Shop"))
            {
                produceToggleButton.gameObject.SetActive(false);
                collectProduceButton.gameObject.SetActive(false);
                enterAbyssButton.gameObject.SetActive(false);

                enterShop.gameObject.SetActive(true);
            }
        }

        else
        {
            enterAbyssButton.gameObject.SetActive(false);
            enterShop.gameObject.SetActive(false);

            produceToggleButton.gameObject.SetActive(true);
            collectProduceButton.gameObject.SetActive(true);
        }
    }

    public void ToggleProduceButton()
    {
        isProducing = !isProducing;
        currPlaceInfo.IsProducing = isProducing;

        ColorBlock color    = produceToggleButton.colors; 
        color.normalColor   = isProducing ? Color.red : Color.white;
        color.selectedColor = isProducing ? Color.red : Color.white;
        produceToggleButton.colors = color;

        produceToggleText.text = isProducing ? "생산 중지" : "생산 시작";

        if (isProducing == false)
        {
            Debug.Log("생산직 퇴근!");

            // 아이템 생산 중지
            currPlaceInfo.gameObject.GetComponent<ProduceCommodities>().StopProduce();
        }

        else
        {
            Debug.Log("아이템 만든다!");

            // 아이템 생산 시작
            currPlaceInfo.gameObject.GetComponent<ProduceCommodities>().StartProduce(currPlaceInfo, isProducing);
        }


    }

    private void OnDisable()
    {
        clickDominionObj.OnUIChanged -= InitUI;
    }
}
