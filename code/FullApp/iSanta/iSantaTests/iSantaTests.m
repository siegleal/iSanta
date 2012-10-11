//
//  iSantaTests.m
//  iSantaTests
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "iSantaTests.h"
#import "PlacementBrain.h"

@implementation iSantaTests

@property (nonatomic, strong) PlacementBrain *brain;

- (void)setUp
{
    [super setUp];
    
    brain = [[PlacementBrain alloc] init];
}

- (void)tearDown
{
    // Tear-down code here.
    
    [super tearDown];
}

- (void)testBrainInit
{
	STAssertEquals(brain.points, nil);
}

- (void)testBrainAddPoint
{
	[brain addPointatX:(5)andY:(5)];
	STAssertEquals([brain.points count], 1);
}

- (void)testBrainReplacePoint
{
	[brain addPointatX:(5)andY:(5)];
	Point *p = [[Point alloc] init];
	p.x = 10;
	p.y = 10;
	STAssertEquals([brain.points count], 1);
	[brain replacePoint:p];
	STAssertEquals([brain.points count], 1);
	p = [brain getObjectAtIndex:0];
	STAssertEquals(p.x, 10);
}

- (void)testRemovePoint
{
	[brain addPointatX:(5)andY:(5)];
	STAssertEquals([brain.points count], 1);
	[brain removePointAtX:(5)andY:(5)];
	STAssertEquals([brain.points count], 0);
}

@end
