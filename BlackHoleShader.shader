﻿Shader "Hidden/BlackHoleShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float2 _Position;
			uniform float _Rad;
			uniform float _Ratio;
			uniform float _Distance;

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata_img v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : COLOR {
				float2 offset = i.uv - _Position;
				float2 ratio = { _Ratio, 1 };
				float rad = length(offset/ratio);
				float deformation = 1 / pow(rad * pow(_Distance, 0.5), 2) * _Rad * 0.1;

				offset = offset * (1 - deformation);
				offset += _Position;

				half4 res = tex2D(_MainTex, offset);

				if (rad * _Distance < _Rad) { 
					res = half4( 0, 0, 0, 1 );
				}

				return res;

			}



			ENDCG
		}
	}
}
