using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 화면 좌표계(Screen Space - Camera / Overlay 둘 다)에서
/// 조이스틱 배경(RectTransform) 안에서만 핸들이 움직이도록 처리하는 조이스틱 스크립트.
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background; // 조이스틱 배경 (움직이지 않는 부분)
    [SerializeField] private RectTransform handle;     // 움직이는 조이스틱 핸들

    // 조이스틱 입력 방향 (정규화된 값, -1 ~ 1)
    public Vector2 InputDirection { get; private set; }

    // 배경 원의 반지름 (UI 로컬 좌표계 기준)
    private float joystickRadius;

    void Start()
    {
        // 실제 렌더링 크기 기준으로 반지름 계산
        joystickRadius = Mathf.Min(background.rect.width, background.rect.height) * 0.5f;

        // 부모(배경)의 로컬 좌표계에서 중앙 위치
        Vector2 center = background.rect.center;

        // 핸들을 배경의 중앙으로 설정
        handle.localPosition = center;
        InputDirection = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 화면 좌표를 배경의 로컬 좌표로 변환
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint))
        {
            return;
        }

        // 배경 중앙을 원점으로 이동
        Vector2 center = background.rect.center;
        Vector2 offsetFromCenter = localPoint - center;

        // 반지름 안으로 클램프 (파란 원 안에서만 움직이도록)
        Vector2 clamped = Vector2.ClampMagnitude(offsetFromCenter, joystickRadius);

        // 핸들 위치 업데이트 (배경 중앙 기준 로컬 좌표)
        handle.localPosition = clamped + center;

        // 입력 방향 정규화 (-1 ~ 1 범위)
        InputDirection = clamped / joystickRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치를 떼면 핸들을 중앙으로 되돌리고 입력 방향 초기화
        handle.localPosition = background.rect.center;
        InputDirection = Vector2.zero;
    }
}
