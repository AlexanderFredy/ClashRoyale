using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardController[] _cardsControllers;
    [SerializeField] private Image _nextCardImage;
    [SerializeField] private int _layerIndex = 6;
    private CardsInGame _cardsInGame;
    private string[] _ids;
    private Camera _camera;
    private List<string> _freeCardIDs;
    private string _nextCardID;

    void Start()
    {
        _camera = Camera.main;
        _ids = new string[_cardsControllers.Length];

        CardsInGame cardsInGame = CardsInGame.Instance;
        _freeCardIDs = cardsInGame.GetAllID();
        MixList(_freeCardIDs);

        for (int i = 0; i < _cardsControllers.Length; i++)
        {
            string cardID = _freeCardIDs[0];
            _freeCardIDs.RemoveAt(0);
            _ids[i] = cardID;
            _cardsControllers[i].Init(this, i, cardsInGame._playerDeck[cardID].sprite);
        }

        SetNextRandomCard();
    }

    private void SetNextRandomCard()
    {
        int randomIndex = UnityEngine.Random.Range(0, _freeCardIDs.Count);
        _nextCardID = _freeCardIDs[randomIndex];
        _freeCardIDs.RemoveAt(randomIndex);
        _nextCardImage.sprite = _cardsInGame._playerDeck[_nextCardID].sprite;
    }

    private void MixList(List<string> cardIDs)
    {
        int length = cardIDs.Count;

        int[] arr = new int[length];
        for (int i = 0; i < length; i++) arr[i] = i;

        System.Random rand = new System.Random();
        arr = arr.OrderBy(x => rand.Next()).ToArray();

        string[] tempArr = new string[length];
        for (int i = 0; i < length; i++) tempArr[i] = cardIDs[i];

        for (int i = 0; i < length; i++) cardIDs[i] = tempArr[i];
    }

    public void Release(int cardControllerIndex, in Vector3 screenPointPosition)
    {
        if (TryGetSpawnPoint(screenPointPosition, out Vector3 screenPoint) == false) return;

        string id = _ids[cardControllerIndex];

        _freeCardIDs.Add(id);

        _ids[cardControllerIndex] = _nextCardID;
        _cardsControllers[cardControllerIndex].SetSprite(_cardsInGame._playerDeck[_nextCardID].sprite);

        SetNextRandomCard();
    }

    private bool TryGetSpawnPoint(Vector3 screenPointPosition, out Vector3 spawnPoint)
    {
        Ray ray = _camera.ScreenPointToRay(screenPointPosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.layer == _layerIndex)
        {
            spawnPoint = hit.point;
            spawnPoint.y = 0;
            return true;
        }
   
        spawnPoint = Vector3.zero;
        return false;
    }
}
