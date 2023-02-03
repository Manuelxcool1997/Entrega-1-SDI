using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Restapimanager : MonoBehaviour
{
    [SerializeField]
    public RawImage character1;
    [SerializeField]
    public RawImage character3;
    [SerializeField]
    public RawImage character5;
    [SerializeField]
    public RawImage character7;
    [SerializeField]
    public RawImage character9;

    [SerializeField]
    private int UserID=1;

    [SerializeField]
    private string ServerApiPath = "https://my-json-server.typicode.com/Manuelxcool1997/Entrega-1-SDI";
    private string RickAndMortyApiURL = "https://rickandmortyapi.com/api";

    IEnumerator GetPlayerINfo()
    {

        UnityWebRequest www = UnityWebRequest.Get(ServerApiPath + "/users/" + UserID);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                UserJsonData User = JsonUtility.FromJson<UserJsonData>(www.downloadHandler.text);
                Debug.Log(User.name);
                foreach (int cardId in User.deck)
                {
                    Debug.Log(cardId);
                }
            }
            else
            {
                string mensaje = "Status" + www.responseCode;
                mensaje += "\ncontent-type" + www.GetResponseHeader("Content-type");

                mensaje += "nError :" + www.error;
                Debug.Log(mensaje);
            }
        }
    }

        IEnumerator GetRickAndMOrtyInfo()
        {

            UnityWebRequest www = UnityWebRequest.Get(RickAndMortyApiURL+"/character/");
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log("NETWORK ERROR :" + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                if (www.responseCode == 200)
                {
                CharacterList characters = JsonUtility.FromJson<CharacterList>(www.downloadHandler.text);
                Debug.Log(characters.info.count);
                foreach (Character character in characters.results)
                {
                    Debug.Log("Name: " + character.name);
                    if(character.id==1)
                    {
                        StartCoroutine(DownloadImage(character.image, character1));
                    }
                    else if (character.id == 3)
                    {
                        StartCoroutine(DownloadImage(character.image, character3));
                    }
                    else if (character.id == 5)
                    {
                        StartCoroutine(DownloadImage(character.image, character5));
                    }
                    else if (character.id == 7)
                    {
                        StartCoroutine(DownloadImage(character.image, character7));
                    }
                    else if (character.id == 9)
                    {
                        StartCoroutine(DownloadImage(character.image, character9));
                    }
                }
            
            }
                else
                {
                    string mensaje = "Status" + www.responseCode;
                    mensaje += "\ncontent-type" + www.GetResponseHeader("Content-type");

                    mensaje += "nError :" + www.error;
                    Debug.Log(mensaje);
                }
            }


        }

    IEnumerator DownloadImage(string MediaURl, RawImage imagen)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaURl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            imagen.texture=((DownloadHandlerTexture) request.downloadHandler).texture;

    }


    public void GetUserOnClick()
    {
        StartCoroutine(GetPlayerINfo());
    }

    public void GetcharacterOnClick()
    {
        StartCoroutine(GetRickAndMOrtyInfo());
    }



}

[System.Serializable]
public class UserJsonData
{
    public int id;
    public string name;
    public int[] deck;
}


[System.Serializable]
public class CharacterList
{
    public CharacterListInfo info;
    public List<Character>results;
}

[System.Serializable]
public class CharacterListInfo
{
    public int count;
    public int pages;
    public string next;
    public string prev;


}

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string image;
}
