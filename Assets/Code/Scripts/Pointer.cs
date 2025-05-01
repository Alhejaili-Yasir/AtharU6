using UnityEngine;

public class ObjectivePointer : MonoBehaviour
{
    [Header("المكان الذي يشير إليه السهم")]
    public Transform target; // هدف المهمة الحالي

    [Header("الشيء الذي سيحمل السهم - مثلاً فوق رأس اللاعب")]
    public Transform player;

    void Update()
    {
        if (target == null || player == null)
            return;

        // احسب الاتجاه من اللاعب إلى الهدف
        Vector3 direction = target.position - player.position;
        direction.y = 0f; // تجاهل الفرق في الارتفاع

        // إذا فيه اتجاه واضح
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }

        // اجعل السهم دائمًا فوق اللاعب
        Vector3 newPos = player.position + new Vector3(0, 2f, 0); // ارفع السهم فوقه قليلاً
        transform.position = newPos;
    }
}
