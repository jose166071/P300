using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum Type
    {
        p300 = 0,
        SSVEP = 1
    }
    
    [SerializeField] public List<GameObject> images;
    [SerializeField] private GameObject _imagenInicial;
    [SerializeField] private List<GameObject> col1;
    [SerializeField] private List<GameObject> col2;
    [SerializeField] private List<GameObject> col3;
    [SerializeField] private List<GameObject> col4;
    [SerializeField] private List<GameObject> col5;
    [SerializeField] private List<GameObject> col6;
    [SerializeField] private List<GameObject> row1;
    [SerializeField] private List<GameObject> row2;
    [SerializeField] private List<GameObject> row3;
    
    
    [SerializeField] private List<List<GameObject>> _rowCol = new List<List<GameObject>>();
    [SerializeField] private float time = 0f;
    
    [SerializeField] public Type _Type;
    
    
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (var imagen  in images)
        {
            SetRowsAndColumns(imagen);
        }
    }

    private void SetRowsAndColumns(GameObject imagen)
    {
        var Pos = imagen.transform.position;
        if (Pos.x == col1[0].transform.position.x && Pos!= col1[0].transform.position)
        {
            col1.Add(imagen);
        }
        else if (Pos.x == col2[0].transform.position.x && Pos!= col2[0].transform.position)
        {
            col2.Add(imagen);
        }
        else if (Pos.x == col3[0].transform.position.x && Pos!= col3[0].transform.position)
        {
            col3.Add(imagen);
        }
        else if (Pos.x == col4[0].transform.position.x && Pos!= col4[0].transform.position)
        {
            col4.Add(imagen);
        }
        else if (Pos.x == col5[0].transform.position.x && Pos!= col5[0].transform.position)
        {
            col5.Add(imagen);
        }
        else if (Pos.x == col6[0].transform.position.x && Pos!= col6[0].transform.position)
        {
            col6.Add(imagen);
        }

        if (Pos.y == row1[0].transform.position.y && Pos!= row1[0].transform.position)
        {
            row1.Add(imagen);
        }
        else if (Pos.y == row2[0].transform.position.y && Pos!= row2[0].transform.position)
        {
            row2.Add(imagen);
        }
        else if (Pos.y == row3[0].transform.position.y && Pos!= row3[0].transform.position)
        {
            row3.Add(imagen);
        }
    }

    void Start()
    {
        SetRowCol();
        foreach (var imagen  in images)
        {
            imagen.SetActive(false);
        }

        if (_Type.Equals(Type.SSVEP))
        {
            SetOscilationTime();
            Debug.Log("SSVEP selected");
            
        }
        else if (_Type.Equals(Type.p300))
        {
            StartCoroutine(ShowInitial());
        }

        
    }

    private void SetOscilationTime()
    {
        foreach (var image in images)
        {
            if (image.TryGetComponent(out ssvepTime freq))
            {
                float oscFreq = freq.Frecuencia;
                Debug.Log(oscFreq);
                StartCoroutine(ssvepBlink(image, oscFreq));
            }
        }
    }

    private IEnumerator ssvepBlink(GameObject image, float oscFreq)
    {
        //Debug.Log("Coroutine Started");
        float per = 1f / oscFreq;
        Debug.Log(per);
        image.SetActive(true);
        yield return new WaitForSeconds(per);
        StartCoroutine(SetOff(image, oscFreq));
        //Debug.Log("Wait time finished");
    }

    private IEnumerator SetOff(GameObject image, float freq)
    {
        image.SetActive(false);
        Debug.Log("Setted Off");
        yield return new WaitForSeconds(1f/freq);
        StartCoroutine(ssvepBlink(image, freq));
        /*if (image == images[17])
        {
            SetOscilationTime();
        }*/
        //StartCoroutine(ssvepBlink(image, freq));
    } 

    private void SetRowCol()
    {
        _rowCol.Add(col1);
        _rowCol.Add(col2);
        _rowCol.Add(col3);
        _rowCol.Add(col4);
        _rowCol.Add(col5);
        _rowCol.Add(col6);
        _rowCol.Add(row1);
        _rowCol.Add(row2);
        _rowCol.Add(row3);
    }

    private void SetToBlack()
    {
        _imagenInicial.SetActive(false);
    }

    private IEnumerator ShowInitial()
    {
        _imagenInicial.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (time<10f)
        {
            SetToBlack();
            StarBlinking();
        }
        else
        {
            SetToBlack();
            StartCoroutine(ShowInitial2());
        }

    }

    private void StarBlinking()
    {
        Debug.Log("StartBlink");
        //StartCoroutine(Blink());
        Blink2();
    }

    private IEnumerator Blink()
    {
        while (time<10f)
        {
            StartCoroutine(CountBlink());
            time += Time.deltaTime;
            Debug.Log(time);
        }
        yield return new WaitForSeconds(10f);
    }
    
    private void Blink2()
    {
        if (time<10f)
        {
            StartCoroutine(CountBlink());
        }
        else
        {
            StartCoroutine(ShowInitial());
        }
        
        /*while (time<10f)
        {
            Debug.Log("Blink2Looping");
            time += Time.deltaTime;
            StartCoroutine(CountBlink());
            //CountBlink2();
        }*/
    }

    private IEnumerator ShowInitial2()
    {
        int rand = Random.Range(0,9);
        int len = _rowCol[rand].Count;
        _imagenInicial = _rowCol[rand][Random.Range(0, len)];
        _imagenInicial.SetActive(true);
        yield return new WaitForSeconds(2f);
        time = 0;
        SetToBlack();
        StarBlinking();
        
    }

    private IEnumerator CountBlink()
    {
        int randomNumber = Random.Range(0,8);
        //Debug.Log(randomNumber);
        foreach (var image in _rowCol[randomNumber])
        {
            image.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        time += 0.1f;
        //setBlackList(_rowCol[randomNumber]);
        StartCoroutine(SetBlackListCoroutine(_rowCol[randomNumber]));
    }

    private IEnumerator SetBlackListCoroutine(List<GameObject> a)
    {
        Debug.Log("Set to False");
        foreach (var image in a)
        {
            image.SetActive(false);
        }

        yield return new WaitForSeconds(0.3f);
        time += 0.3f;
        Blink2();
    }

    private void CountBlink2()
    {
        int randomNumber = Random.Range(0,9);
        Debug.Log(randomNumber);
        foreach (var image in _rowCol[randomNumber])
        {
            image.SetActive(true);
        }
        setBlackList(_rowCol[randomNumber]);
    }

    private void setBlackList(List<GameObject> a)
    {
        Debug.Log("Set to False");
        foreach (var image in a)
        {
            image.SetActive(false);
        }

        Blink2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
