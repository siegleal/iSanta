//
//  Test_Report.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Ammunition_Information;

@interface Test_Report : NSManagedObject

@property (nonatomic, retain) NSDate * date_Time;
@property (nonatomic, retain) Ammunition_Information *test_Ammunition;
@property (nonatomic, retain) NSManagedObject *test_Photo;
@property (nonatomic, retain) NSManagedObject *test_Range;
@property (nonatomic, retain) NSManagedObject *test_Shooter;
@property (nonatomic, retain) NSManagedObject *test_Weapon;

@end
