using UnityEngine;
using TMPro;

public class MarqueeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float margin = 10;
    public float speed;
    public float waitTime;

    private float _startX;
    private float _textWidth;
    private bool _overflowing;
    private RectTransform _textRect;
    private TextMeshProUGUI _textLoopCopy;
    private float _waitUntil;
    
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        text.ForceMeshUpdate();
        _startX = text.rectTransform.anchoredPosition.x;
        _textWidth = text.preferredWidth;
        _textRect = text.rectTransform;
        _overflowing = (text.textBounds.size.x > text.rectTransform.rect.size.x);
        //Debug.Log(text.text + (_overflowing ? "is overflowing" : "is within bounds"));
        if (_textLoopCopy)
        {
            Destroy(_textLoopCopy);
        }
        if (_overflowing) {
            _textLoopCopy = Instantiate(text, text.transform.parent);
            _textLoopCopy.rectTransform.anchoredPosition = 
                new Vector2(_startX + _textWidth + margin, _textRect.anchoredPosition.y);
        }
    }

    private void Update()
    {
        if (!_overflowing || !_textLoopCopy) return;
        if (Time.time < _waitUntil) return;
        
        _textRect.anchoredPosition += Vector2.left * (speed * Time.deltaTime);
        _textLoopCopy.rectTransform.anchoredPosition += Vector2.left * (speed * Time.deltaTime);
        
        if (_startX - _textRect.anchoredPosition.x > _textWidth + margin)
        {
            _textRect.anchoredPosition = 
                new Vector2(_startX + _textWidth + margin, _textRect.anchoredPosition.y);
            _waitUntil = Time.time + waitTime;
        }
        
        if (_startX - _textLoopCopy.rectTransform.anchoredPosition.x > _textWidth + margin)
        {
            _textLoopCopy.rectTransform.anchoredPosition = 
                new Vector2(_startX + _textWidth + margin, _textRect.anchoredPosition.y);
            _waitUntil = Time.time + waitTime;
        }
    }
}
