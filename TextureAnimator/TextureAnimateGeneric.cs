using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TextureAnimator
{

    public class TextureAnimateGeneric : PartModule
    {
        Transform _mesh;

        [KSPField]
        public string ObjectName;
        [KSPField]
        int speedU = 15;
        [KSPField]
        int speedV = 30;
        [KSPField]
        public float smoothSpeed = 10;

        int timeU;
        int timeV;

        float offsetU;
        float offsetV;

        float smoothedU;
        float smoothedV;

        [KSPField]
        float minOffsetU = -.1f; //default valuesm, over-ridden in config
        [KSPField]
        float maxOffsetU = .1f;

        [KSPField]
        float minOffsetV = -.1f;
        [KSPField]
        float maxOffsetV = .1f;

        [KSPField]
        public bool isReady = false;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            //print(Version.versionNumber);

            _mesh = transform.Search(ObjectName);

            if (HighLogic.LoadedSceneIsFlight)
                isReady = true;

        }

        public void Update()
        {
            if (!isReady)
                return;

            if (timeU <= 0)
            {
                timeU = UnityEngine.Random.Range(0, speedU);    //generate a random value up to configured speed
                offsetU = UnityEngine.Random.Range(minOffsetU, maxOffsetU); //generate a random value between configured values
            }

            if (timeV <= 0)
            {
                timeV = UnityEngine.Random.Range(0, speedV);
                offsetV = UnityEngine.Random.Range(minOffsetV, maxOffsetV);
            }

            timeU--;    //decrement time every frame
            timeV--;

            smoothedU = Mathf.Lerp(smoothedU, offsetU, Time.deltaTime * smoothSpeed);   //smooth movement between the random values

            smoothedV = Mathf.Lerp(smoothedV, offsetV, Time.deltaTime * smoothSpeed);

            Material material = _mesh.renderer.material;    //set things up for changing the texture offset on the track
            Vector2 textureOffset = material.mainTextureOffset;
            textureOffset = new Vector2(smoothedU, smoothedV); //Set the values for current texture offset
            material.SetTextureOffset("_MainTex", textureOffset);   //change main tex offset
            material.SetTextureOffset("_BumpMap", textureOffset);   //change bump map offset
        }
    }
}