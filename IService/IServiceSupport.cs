﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IService
{
    /// <summary>
    /// 一个标识接口，所有服务都必须实现这个接口。这样可以保证只有真正的服务实现类才会被注册到Autofac
    /// </summary>
    public interface IServiceSupport
    {
    }
}
