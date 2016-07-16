![Icon](http://imgur.com/n8AFOnp.png) 
# Equalizer [![Build status](https://ci.appveyor.com/api/projects/status/sj7voayxy3c002an?svg=true)](https://ci.appveyor.com/project/lvermeulen/equalizer) [![license](https://img.shields.io/github/license/lvermeulen/Equalizer.svg?maxAge=2592000)](https://github.com/lvermeulen/Equalizer/blob/master/LICENSE) [![NuGet](https://img.shields.io/nuget/v/Equalizer.Core.svg?maxAge=2592000)](https://www.nuget.org/packages/Equalizer.Core/)  ![](https://img.shields.io/badge/.net-4.5.1-yellowgreen.svg) ![](https://img.shields.io/badge/netstandard-1.6-yellowgreen.svg)
Equalizer is an extensible library for choosing the next item from a list of items, also known as load balancing.

##Features:
* RandomRouter: chooses a random next item
* RoundRobinRouter: chooses the next item in round robin fashion
* FailOverRouter: chooses specific item if available, otherwise a different one
* RoundRobinAddressRouter for [Nanophone](https://github.com/lvermeulen/Nanophone): chooses the next item with a different address

##Thanks
* [balance](https://thenounproject.com/term/balance/203869/) icon by [Golden Roof](https://thenounproject.com/goldenroof/) from [The Noun Project](https://thenounproject.com)
