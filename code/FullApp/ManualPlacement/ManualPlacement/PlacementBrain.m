//
//  PlacementBrain.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "PlacementBrain.h"


@implementation PlacementBrain
@synthesize targetImage = _targetImage;
@synthesize circleImage = _circleImage;
@synthesize points = _points;

- (UIImage *)targetImage
{
    /*get image*/
    //
    //
    
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:@"TakePhoto" 
                                         ofType:@"png"];
    //
    //
    //
    if (!_targetImage) _targetImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _targetImage;
}

- (UIImage *)circleImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:@"circle" 
                                         ofType:@"png"];
    if (!_circleImage) _circleImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _circleImage;
}

- (void)printPoints
{
    NSEnumerator *e = [self.points objectEnumerator];
    NSValue *i = [e nextObject];
    NSString *output = @"";
    while(i)
    {
        output = [output stringByAppendingFormat:@" %f, %f ;",i.CGPointValue.x,i.CGPointValue.y];
        i = [e nextObject];
    }  
    NSLog(@"%@",output);
}

- (NSMutableArray *)points
{
    if (!_points) _points = [[NSMutableArray alloc] init];
    return _points;
}

- (void)addPointatX:(CGFloat)x andY:(CGFloat)y
{
    NSValue *pointToAdd = [NSValue valueWithCGPoint:CGPointMake(x, y)];
    [self.points addObject:pointToAdd];
    [self printPoints];
}

- (void)removePointatX:(int)x andY:(int)y
{
    NSEnumerator *e = [self.points objectEnumerator];
    NSValue *i = e.nextObject;
    while(i)
    {
        if (i.CGPointValue.x == x && i.CGPointValue.y == y)
        {
            [self.points removeObject:i];
        }
        i = e.nextObject;
    }
}




@end
