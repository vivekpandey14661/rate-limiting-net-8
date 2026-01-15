local key = KEYS[1]
local capacity = tonumber(ARGV[1])
local refill_rate = tonumber(ARGV[2])
local now = tonumber(ARGV[3])

local data = redis.call("HMGET", key, "tokens", "timestamp")

local tokens = tonumber(data[1]) or capacity
local last = tonumber(data[2]) or now

local delta = math.max(0, now - last)
tokens = math.min(capacity, tokens + delta * refill_rate)

if tokens < 1 then
  redis.call("HMSET", key, "tokens", tokens, "timestamp", now)
  return 0
end

tokens = tokens - 1
redis.call("HMSET", key, "tokens", tokens, "timestamp", now)
return 1
