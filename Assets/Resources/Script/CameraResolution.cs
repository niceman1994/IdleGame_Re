using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    int width;
    int height;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        width = Screen.width;
        height = Screen.height;
        Screen.SetResolution(width, height, true);
    }
}

// ((float)Screen.width / Screen.height) => 가진 핸드폰의 가로 / 세로, ((float)9 / 16) => 고정하려는 화면 비율
// (가로 / 세로), 정수가 아닐경우 소수점을 없애기때문에 float를 붙였다
// 가로로 눕혀서 할 경우 ((float)9 / 16)가 아닌 ((float)16 / 9)로 해야한다.
// 고정한 가로비율 scaleheight이보다 크면 나눴을 때 1보다 큰 값이 나오고 scaleheight이보다 작으면 1보다 작은 값이 나온다.
//float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); 
//float scalewidth = 1f / scaleheight;
//if (scaleheight < 1)    // 고정하려는 화면 비율보다 유니티에서 설정한 비율이 작으면 위아래가 남는다.
//{
//    rect.height = scaleheight;          // 위아래가를 조절해 화면을 맞춘다.
//    rect.y = (1f - scaleheight) / 2f;   // y 값을 조절해 남는 공간을 없앤다.
//}
//else                    // 고정하려는 화면 비율보다 유니티에서 설정한 비율이 크면 좌우가 남는다.
//{
//    rect.width = scalewidth;            // 좌우를 조절해 화면을 맞춘다.
//    rect.x = (1f - scalewidth) / 2f;    // x 값을 조절해 남는 공간을 없앤다.
//}
//camera.rect = rect;     // 바뀐 rect를 카메라 rect에 다시 대입한다.