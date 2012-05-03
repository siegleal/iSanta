//
//  Test_Report.h
//  iSanta
//
//  Created by  on 5/3/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Ammunition_Information, Photo_Information, Range_Information, Shooter_Information, Weapon_Information;

@interface Test_Report : NSManagedObject

@property (nonatomic, retain) NSDate * date_Time;
@property (nonatomic, retain) Ammunition_Information *test_Ammunition;
@property (nonatomic, retain) Photo_Information *test_Photo;
@property (nonatomic, retain) Range_Information *test_Range;
@property (nonatomic, retain) Shooter_Information *test_Shooter;
@property (nonatomic, retain) Weapon_Information *test_Weapon;

@end
