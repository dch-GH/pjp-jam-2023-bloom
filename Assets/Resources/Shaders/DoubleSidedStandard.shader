Shader "Custom/DoubleSidedStandard" {
    Properties {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Cutoff ("Alpha cutoff", Range (0.0, 1.0)) = 0.5
    }

    SubShader {
        Tags {"Queue"="Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        fixed4 _Color;
        float _Cutoff;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            clip(c.a - _Cutoff);
        }
        ENDCG
    }

    SubShader {
        Tags {"Queue"="Overlay" }
        LOD 100

        Cull Front

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        fixed4 _Color;
        float _Cutoff;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            clip(c.a - _Cutoff);
        }
        ENDCG
    }

    SubShader {
        Tags {"Queue"="Overlay" }
        LOD 100

        Cull Off

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        fixed4 _Color;
        float _Cutoff;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            clip(c.a - _Cutoff);
        }
        ENDCG
    }

    // Add more SubShaders if needed for different platform support
    // ...

    Fallback "Diffuse"
}
