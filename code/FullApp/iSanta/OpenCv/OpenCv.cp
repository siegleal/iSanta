/*
 *  OpenCv.cp
 *  OpenCv
 *
 *  Created by  on 5/7/12.
 *  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
 *
 */

#include <iostream>
#include "OpenCv.h"
#include "OpenCvPriv.h"

void OpenCv::HelloWorld(const char * s)
{
	 OpenCvPriv *theObj = new OpenCvPriv;
	 theObj->HelloWorldPriv(s);
	 delete theObj;
};

void OpenCvPriv::HelloWorldPriv(const char * s) 
{
	std::cout << s << std::endl;
};

