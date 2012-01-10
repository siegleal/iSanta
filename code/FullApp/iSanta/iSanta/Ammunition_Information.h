//
//  Ammunition_Information.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>


@interface Ammunition_Information : NSManagedObject

@property (nonatomic, retain) NSNumber * caliber;
@property (nonatomic, retain) NSNumber * lot_Number;
@property (nonatomic, retain) NSString * ammunition_Notes;
@property (nonatomic, retain) NSNumber * number_Of_Shots;
@property (nonatomic, retain) NSNumber * projectile_Mass;
@property (nonatomic, retain) NSManagedObject *test;

@end
