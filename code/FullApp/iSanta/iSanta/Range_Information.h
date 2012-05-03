//
//  Range_Information.h
//  iSanta
//
//  Created by  on 5/3/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Range_Information : NSManagedObject

@property (nonatomic, retain) NSNumber * distance_To_Target;
@property (nonatomic, retain) NSString * firing_Range;
@property (nonatomic, retain) NSString * range_Temperature;
@property (nonatomic, retain) Test_Report *test;

@end
