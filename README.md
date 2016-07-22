![Icon](/assets/noun_203869_cc.png?raw=true) 
# Equalizer [![Build status](https://ci.appveyor.com/api/projects/status/sj7voayxy3c002an?svg=true)](https://ci.appveyor.com/project/lvermeulen/equalizer) [![license](https://img.shields.io/badge/license-MIT-blue.svg?maxAge=2592000)](https://github.com/lvermeulen/Equalizer/blob/master/LICENSE) [![NuGet](https://img.shields.io/nuget/vpre/Equalizer.Core.svg?maxAge=2592000)](https://www.nuget.org/packages/Equalizer.Core/)  ![](https://img.shields.io/badge/.net-4.5.1-yellowgreen.svg) ![](https://img.shields.io/badge/netstandard-1.6-yellowgreen.svg)

[![Join the chat at https://gitter.im/lvermeulen/Equalizer](https://badges.gitter.im/lvermeulen/Equalizer.svg)](https://gitter.im/lvermeulen/Equalizer?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
Equalizer is an extensible library for choosing the next item from a list of items.

##Features:
* CallbackRouter: chooses the next item based on the provided callback
* FailOverRouter: chooses specific item if available, otherwise a different one
* RandomRouter: chooses a random next item
* RoundRobinRouter: chooses the next item in round robin fashion
* RoundRobinAddressRouter for [Nanophone](https://github.com/lvermeulen/Nanophone): chooses the next service instance with a different address
* Load balancing middleware for aspnetcore: redirects requests to the next chosen service instance

##Usage:
* CallbackRouter:
~~~~
    var first = "1";
    var second = "2";
    var third = "3";
    var instances = new List<string> { first, second, third };

    var router = new CallbackRouter<string>(x => "3");

    // choose third
    var next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("3", next);
~~~~

* FailOverRouter:
~~~~
    var first = "1";
    var second = "2";
    var third = "3";
    var instances = new List<string> { first, second, third };

    var router = new FailOverRouter<string>(first, isAvailable: x => false);

    // don't choose first
    var next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.NotEqual("1", next);
~~~~

* RandomRouter:
~~~~
    var first = "1";
    var second = "2";
    var third = "3";
    var instances = new List<string> { first, second, third };

    var router = new RandomRouter<string>();

    // don't return null
    var next = router.Choose(instances);
    Assert.NotNull(next);
~~~~

* RoundRobinRouter:
~~~~
    var first = "1";
    var second = "2";
    var third = "3";
    var instances = new List<string> { first, second, third };

    var router = new RoundRobinRouter<string>();

    // choose first
    var next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("1", next);

    // choose next
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("2", next);

    // choose next
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("3", next);

    // choose first
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("1", next);
~~~~

* RoundRobinAddressRouter:
~~~~
    var oneDotOne = new RegistryInformation("1", 1234, "some version");
    var oneDotTwo = new RegistryInformation("1", 1234, "some version");
    var twoDotOne = new RegistryInformation("2", 1234, "some version");
    var twoDotTwo = new RegistryInformation("2", 1234, "some version");
    var threeDotOne = new RegistryInformation("3", 1234, "some version");
    var threeDotTwo = new RegistryInformation("3", 1234, "some version");
    var instances = new List<RegistryInformation> 
	{ 
		oneDotOne, oneDotTwo, 
		twoDotOne, twoDotTwo, 
		threeDotOne, threeDotTwo 
	};

    var router = new RoundRobinAddressRouter();

    // choose first
    var next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("1", next.Address);

    // choose next
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("2", next.Address);

    // choose next
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("3", next.Address);

    // choose first
    next = router.Choose(instances);
    Assert.NotNull(next);
    Assert.Equal("1", next.Address);
~~~~

* Middleware:
~~~~
	// create registry client
    var registryClient = new RegistryClient("my-url-path-prefix-", new RoundRobinAddressRouter());

	// add Consul registry host
    var consul = new ConsulRegistryHost();
    registryClient.AddRegistryHost(consul);

	// add in-memory registry host
	var inmemory = new InMemoryRegistryHost();
	registryClient.AddRegistryHost(inmemory);

	// use IApplicationBuilder extension
    app.UseEqualizer(new EqualizerMiddlewareOptions { RegistryClient = registryClient });
~~~~

##Thanks
* [balance](https://thenounproject.com/term/balance/203869/) icon by [Golden Roof](https://thenounproject.com/goldenroof/) from [The Noun Project](https://thenounproject.com)
