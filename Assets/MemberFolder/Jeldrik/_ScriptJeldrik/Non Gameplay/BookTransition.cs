using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookTransition : MonoBehaviour
{
    #region References
    private Image _book;
    private QuestionManager _qM;
    #endregion

    void Start()
    {
        _book = GetComponent<Image>();
        _book.transform.localScale = Vector3.zero;
        _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
    }

    // Scales the book image up until it fills the whole screen and then loads back into the main menu scene
    public void OpenBook()
    {
        _book.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, _book.transform.position.z);
        _book.transform.DOScale(new Vector3(6.7f, 6.7f, 6.7f), 2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            SceneManager.LoadScene("Book");
        });
    }
}
