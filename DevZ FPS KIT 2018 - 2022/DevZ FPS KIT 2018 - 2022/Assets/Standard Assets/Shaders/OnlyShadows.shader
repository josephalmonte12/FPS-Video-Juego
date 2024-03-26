Shader "OnlyShadows"{
    Subshader{
		UsePass "VertexLit/SHADOWCOLLECTOR"
		UsePass "VertexLit/SHADOWCASTER"
    }
   
    Fallback off
}
