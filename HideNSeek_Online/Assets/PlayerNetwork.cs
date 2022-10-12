using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private float speed = 3f;

    private NetworkVariable<CustomData> randomNumber = new NetworkVariable<CustomData>(
        new CustomData
        {
            _int = 47,
            _bool = true
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
        );

    private struct CustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }

    public override void OnNetworkDespawn()
    {
        randomNumber.OnValueChanged += (CustomData previousValue, CustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + "CustomDataValue : " + newValue._int + "; " + newValue._bool);
        };
    }
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = new CustomData
            {
                _int = 10,
                _bool = false
            };
        }

        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1f;
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;


        transform.position += moveDir * speed * Time.deltaTime;



    }
}
