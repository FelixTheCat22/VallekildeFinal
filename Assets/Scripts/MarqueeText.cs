using UnityEngine;
using TMPro;

public class MarqueeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float margin = 10;
    public float speed;
    public float waitTime;

    public bool overflowing { get; private set; }
    public float textWidth { get; private set; }
        
    private float _startX;
    private RectTransform _textRect;
    private TextMeshProUGUI _textLoopCopy;
    private float _waitUntil;
    private bool _initialized;
    private float _defaultWaitTime;

    private void Awake()
    {
        _defaultWaitTime = waitTime;
    }
    
    private void Start()
    {
        if (!_initialized) Initialize();
    }

    public void Initialize()
    {
        text.ForceMeshUpdate();
        
        if (!_initialized)
        {
            _startX = text.rectTransform.anchoredPosition.x;
        }
        else
        {
            text.rectTransform.anchoredPosition = new Vector2(_startX, text.rectTransform.anchoredPosition.y);
        }

        _waitUntil = 0;
        waitTime = _defaultWaitTime;
        _initialized = true;
        textWidth = text.preferredWidth;
        _textRect = text.rectTransform;
        overflowing = (text.textBounds.size.x > text.rectTransform.rect.size.x);
        //Debug.Log(text.text + (_overflowing ? "is overflowing" : "is within bounds"));
        if (_textLoopCopy)
        {
            Destroy(_textLoopCopy.gameObject);
        }
        if (overflowing) {
            _textLoopCopy = Instantiate(text, text.transform.parent);
            _textLoopCopy.rectTransform.anchoredPosition = 
                new Vector2(_startX + textWidth + margin, _textRect.anchoredPosition.y);
        }
    }

    private void Update()
    {
        if (!overflowing || !_textLoopCopy) return;
        if (Time.time < _waitUntil) return;
        
        _textRect.anchoredPosition += Vector2.left * (speed * Time.deltaTime);
        _textLoopCopy.rectTransform.anchoredPosition += Vector2.left * (speed * Time.deltaTime);
        
        if (_startX - _textRect.anchoredPosition.x > textWidth + margin)
        {
            _textRect.anchoredPosition = 
                new Vector2(_startX + textWidth + margin, _textRect.anchoredPosition.y);
            _waitUntil = Time.time + waitTime;
        }
        
        if (_startX - _textLoopCopy.rectTransform.anchoredPosition.x > textWidth + margin)
        {
            _textLoopCopy.rectTransform.anchoredPosition = 
                new Vector2(_startX + textWidth + margin, _textRect.anchoredPosition.y);
            _waitUntil = Time.time + waitTime;
        }
    }
}
