using System;
using UnityEngine;

namespace Voxels
{
    public class MapEditorCamera : MonoBehaviour
    {
        public float speed = 10f;
        public BlockId block = BlockId.Stone;
        bool adjacent = true;

        Vector3 pos;
        Vector2 rot;

        void Start()
        {
            // Offset camera just as much as the inspector says so when we
            // first click the Game window nothing everything is fine.
            var angles = transform.rotation.eulerAngles;
            rot = new Vector2(angles.y, -angles.x);
        }

        void Update()
        {
            Cursor.lockState = CursorLockMode.Confined;

            HandleCameraPosition();
            HandleCameraRotation();

            MousePick(KeyCode.Mouse0, (hit) =>
            {
                if (hit.collider == null) return;

                var chunk = hit.transform.GetComponent<Chunk>();
                if (chunk == null) return;

                // var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // go.transform.position = hit.point;
                // go.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.25f);
                // Destroy(go, 1f);

                var pos = GetBlockPos(hit, adjacent);
                // Debug.LogFormat("Hit point={0} world_pos={1} normal={2}", hit.point, pos, hit.normal);
                // Debug.DrawRay(pos.ToVector3(), hit.normal, Color.yellow * 10f, 2.5f);

                MapEditor.Instance.world.SetBlock(pos.x, pos.y, pos.z, block);
            });

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                adjacent = !adjacent;
            }

            ChangeBlock(KeyCode.Alpha1, BlockId.Cobblestone, "cobblestone");
            ChangeBlock(KeyCode.Alpha2, BlockId.Stone, "stone");
            ChangeBlock(KeyCode.Alpha3, BlockId.Dirt, "dirt");
            ChangeBlock(KeyCode.Alpha4, BlockId.Grass, "grass");
            ChangeBlock(KeyCode.Alpha5, BlockId.Sand, "sand");
            ChangeBlock(KeyCode.Alpha9, BlockId.Error, "error");
            ChangeBlock(KeyCode.Alpha0, BlockId.Air, "air");

            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Saving world...");
                MapEditor.Instance.SaveWorld();
            }
        }

        void ChangeBlock(KeyCode keyCode, BlockId block, string name)
        {
            if (Input.GetKeyDown(keyCode))
            {
                this.block = block;
                Debug.LogFormat("Changing block to {0}", name);
            }
        }

        void HandleCameraPosition()
        {
            if (!Input.GetKey(KeyCode.Mouse1)) return;

            pos = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) pos.z = speed;
            if (Input.GetKey(KeyCode.S)) pos.z = -speed;
            if (Input.GetKey(KeyCode.A)) pos.x = -speed;
            if (Input.GetKey(KeyCode.D)) pos.x = speed;
            if (Input.GetKey(KeyCode.Q)) pos.y = -speed;
            if (Input.GetKey(KeyCode.E)) pos.y = speed;

            transform.Translate(pos * Time.deltaTime);
        }

        void HandleCameraRotation()
        {
            if (!Input.GetKey(KeyCode.Mouse1)) return;

            rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);

            transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

            // transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
            // transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        }

        void MousePick(KeyCode keyCode, Action<RaycastHit> action, float distance = 100f)
        {
            if (!MapEditor.Press(keyCode)) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green, 1f);

            RaycastHit hit;
            if (!Physics.Raycast(ray.origin, ray.direction, out hit, distance)) return;
            Debug.DrawRay(hit.point, Vector3.up, Color.blue, 1f);

            action(hit);
        }

        private WorldPos GetBlockPos(Vector3 pos)
        {
            WorldPos blockPos = new WorldPos(
                Mathf.RoundToInt(pos.x),
                Mathf.RoundToInt(pos.y),
                Mathf.RoundToInt(pos.z)
            );

            return blockPos;
        }

        private WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            Vector3 pos = new Vector3(
                MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
                MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
                MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

            return GetBlockPos(pos);
        }

        private float MoveWithinBlock(float pos, float norm, bool adjacent = false)
        {
            if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
            {
                if (adjacent)
                {
                    pos += (norm / 2);
                }
                else
                {
                    pos -= (norm / 2);
                }
            }

            return (float)pos;
        }
    }
}
