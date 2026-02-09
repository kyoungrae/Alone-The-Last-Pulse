using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background; // 조이스틱 배경 (움직이지 않는 부분)
    [SerializeField] private RectTransform handle;     // 움직이는 조이스틱 핸들

    public Vector2 InputDirection { get; private set; } // 조이스틱 입력 방향 (정규화된 값)

    private Vector2 joystickCenter;
    private float joystickRadius;

    void Start()
    {
        // 조이스틱 배경의 중앙 위치와 반경 계산
        joystickCenter = background.position;
        joystickRadius = background.sizeDelta.x / 2f;
        
        // 초기 핸들 위치를 중앙으로 설정
        handle.position = joystickCenter;
        InputDirection = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPosition = eventData.position; // 현재 터치 위치

        // 조이스틱 배경의 중앙에서 터치 위치까지의 벡터 계산
        Vector2 direction = touchPosition - joystickCenter;

        // 방향 벡터의 길이가 조이스틱 반경을 초과하지 않도록 제한
        if (direction.magnitude > joystickRadius)
        {
            direction = direction.normalized * joystickRadius;
        }

        // 핸들 위치 업데이트
        handle.position = joystickCenter + direction;

        // 입력 방향 정규화 (0~1 범위)
        InputDirection = direction / joystickRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치를 떼면 핸들을 중앙으로 되돌리고 입력 방향 초기화
        handle.position = joystickCenter;
        InputDirection = Vector2.zero;
    }
}
