//
//  Ammunition_Information.h
//  iSanta
//
//  Created by  on 5/3/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Ammunition_Information : NSManagedObject

@property (nonatomic, retain) NSString * ammunition_Notes;
@property (nonatomic, retain) NSNumber * caliber;
@property (nonatomic, retain) NSNumber * lot_Number;
@property (nonatomic, retain) NSNumber * number_Of_Shots;
@property (nonatomic, retain) NSNumber * projectile_Mass;
@property (nonatomic, retain) Test_Report *test;

@end
