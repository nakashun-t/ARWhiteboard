﻿/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

using System.Collections.Generic;

namespace NRKernal.Record
{
    public class FrameCaptureContextFactory
    {
        private static List<FrameCaptureContext> m_ContextList = new List<FrameCaptureContext>();

        public static FrameCaptureContext Create()
        {
#if UNITY_EDITOR
            AbstractFrameProvider provider = new EditorFrameProvider();
#else
            AbstractFrameProvider provider = new RGBCameraFrameProvider();
#endif
            FrameCaptureContext context = new FrameCaptureContext(provider);

            m_ContextList.Add(context);
            return context;
        }

        public static void DisposeAllContext()
        {
            foreach (var item in m_ContextList)
            {
                if (item != null)
                {
                    item.StopCapture();
                    item.Release();
                }
            }
        }
    }
}
