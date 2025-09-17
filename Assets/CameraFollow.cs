using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;          // Nhân vật (Jerry)
    public Vector3 offset = new Vector3(0, 4, -6); // vị trí camera so với nhân vật
    public float followSpeed = 5f;    // tốc độ bám theo
    public float lookAhead = 2f;      // nhìn về phía trước (tạo cảm giác chạy nhanh)

    private Vector3 targetPosition;

    void LateUpdate() {
        if (!target) return;

        // Tính vị trí mục tiêu của camera
        targetPosition = target.position + offset;

        // Dịch chuyển mượt về vị trí mới
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Hướng camera nhìn về phía trước (tạo cảm giác chạy theo)
        Vector3 lookPoint = target.position + target.forward * lookAhead;
        transform.LookAt(lookPoint);
    }
}
