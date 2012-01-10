//
//  Range_Information.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Range_Information : NSManagedObject

@property (nonatomic, retain) NSString * distance_To_Target;
@property (nonatomic, retain) NSString * firing_Range;
@property (nonatomic, retain) NSNumber * range_Temperature;
@property (nonatomic, retain) Test_Report *test;

@end
