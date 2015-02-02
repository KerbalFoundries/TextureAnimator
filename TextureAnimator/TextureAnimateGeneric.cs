//Tecture animator by Lo-Fi CC-BY-SA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TextureAnimator
{

    public class TextureAnimateGeneric : PartModule
    {
        //config fields
        [KSPField]
        public string ObjectName;
        [KSPField]
        public int minSpeedU = 5;
        [KSPField]
        public int maxSpeedU = 15; //default valuesm, over-ridden in config
        [KSPField]
        public int minSpeedV = 0; //default valuesm, over-ridden in config
        [KSPField]
        public int maxSpeedV = 30;
        [KSPField]
        public float smoothSpeed = 10;
        [KSPField]
        public float minOffsetU = -.1f; 
        [KSPField]
        public float maxOffsetU = .1f;
        [KSPField]
        public float minOffsetV = -.1f;
        [KSPField]
        public float maxOffsetV = .1f;
        [KSPField]
        public bool additiveMode = false;
         
        //internal global variables
        int timeU;
        int timeV;

        float offsetU;
        float offsetV;

        float smoothedU;
        float smoothedV;

        //public global variables
        public bool isReady = false;

        //objects
        Transform _mesh;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            //print(Version.versionNumber);
            
            if (HighLogic.LoadedSceneIsFlight)
                isReady = true;

            try
            {
                _mesh = transform.Search(ObjectName);
            }
            catch
            {
                isReady = false;
            }
            print("starting texture animator");

        }

        public void Update()
        {
            if (!isReady)
                return;

            if (timeU <= 0)
            {
                timeU = UnityEngine.Random.Range(minSpeedU, maxSpeedU);    //generate a random value up to configured speed
                offsetU = UnityEngine.Random.Range(minOffsetU, maxOffsetU); //generate a random value between configured values
            }

            if (timeV <= 0)
            {
                timeV = UnityEngine.Random.Range(minSpeedV, maxSpeedV);
                offsetV = UnityEngine.Random.Range(minOffsetV, maxOffsetV);
            }

            timeU--;    //decrement time every frame
            timeV--;

            smoothedU = Mathf.Lerp(smoothedU, offsetU, Time.deltaTime * smoothSpeed);   //smooth movement between the random values

            smoothedV = Mathf.Lerp(smoothedV, offsetV, Time.deltaTime * smoothSpeed);

            Material material = _mesh.renderer.material;    //set things up for changing the texture offset on the track
            Vector2 textureOffset = material.mainTextureOffset;
            if(additiveMode)
                textureOffset = textureOffset + new Vector2(smoothedU, smoothedV); //Set the values for current texture offset
            else
                textureOffset = new Vector2(smoothedU, smoothedV); //Set the values for current texture offset
            material.SetTextureOffset("_MainTex", textureOffset);   //change main tex offset
            material.SetTextureOffset("_BumpMap", textureOffset);   //change bump map offset
        }
    }
}
